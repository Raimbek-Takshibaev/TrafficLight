using Newtonsoft.Json;
using Observer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Observer
{
    class Init
    {
        private string _seqId;
        private string _url;
        private int _trafficLightCounter = 0;
        public Init(string url) => _url = url;      
        public void Start()
        {
            
                string makeResult = MakeEmptyRequest("POST", "/sequence/create");
                AddResponse addResponse = JsonConvert.DeserializeObject<AddResponse>(makeResult);
                _seqId = addResponse.Response.Sequence;
                while (true)
                {
                    string getObsResultJson = MakeEmptyRequest("GET", "/sequence/get", $"/?id={_seqId}");
                    GetResponse getResponse = JsonConvert.DeserializeObject<GetResponse>(getObsResultJson);
                    Console.WriteLine(getObsResultJson);
                    if(getResponse.TrafficLight.Color == "red")
                    {
                        if(_trafficLightCounter > 0)
                        {
                            Console.WriteLine(MakeRequest
                                (
                                    JsonConvert.SerializeObject(new Request()
                                    {
                                        Sequence = _seqId,
                                        Observation = new Observation() { Color = "red" }
                                    }),
                                    "POST"
                                ));
                            break;
                        }
                        Thread.Sleep(1000);
                        continue;
                    }
                    Request request = new Request()
                    {
                        Sequence = _seqId,
                        Observation = new Observation()
                        {
                            Color = getResponse.TrafficLight.Color,
                            Numbers = getResponse.TrafficLight.Clock
                        }
                    };
                    string obsResponseJson = MakeRequest(JsonConvert.SerializeObject(request), "POST");
                    ObsResponse obsResponse = JsonConvert.DeserializeObject<ObsResponse>(obsResponseJson);
                    Console.WriteLine($"Traffic light's start time - {ToStringNumbers(obsResponse.Response.Start)}. Missing numbers - {obsResponse.Response.Missing[0]} {obsResponse.Response.Missing[1]}");
                    if(obsResponse.Response.Start.Length == 1)
                    {
                        break;
                    }
                    _trafficLightCounter++;
                    Thread.Sleep(300);
                }
               
        }
        public string ToStringNumbers(int[] nums)
        {
            string numsStr = "";
            for (int i = 0; i < nums.Length; i++)
            {
                numsStr += nums[i] + " ";
            }
            return numsStr;
        }
        public string MakeEmptyRequest(string method, string path, string routeParams = "")
        {
            WebRequest request = WebRequest.Create(_url + path + routeParams);
            request.ContentType = "application/json";
            request.Method = method;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }
        public string MakeRequest(string json, string method)
        {
            WebRequest request = WebRequest.Create(_url + "/observation/add");
            request.Method = method; 
            string data = json;
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
