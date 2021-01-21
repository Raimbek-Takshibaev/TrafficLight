using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficLightAPI.Models;

namespace TrafficLightAPI.Services
{
    public class SequencesService
    {
        // 0 1 2 3 4 5 6 7 8 9
        public static string[] clocks = new string[10] {"1110111", "0010010", "1011101", "1011011",  "0111010", "1101011",
            "1101111", "1010010", "1111111", "1111011"};
        public Sequence GenerateSequence()
        {
            Sequence sequence = new Sequence();
            Random rndm = new Random();
            if (rndm.Next(2) == 0)
                sequence.StartColor = "green";
            else
                sequence.StartColor = "red";
            sequence.MaxClock = rndm.Next(2, 99);
            sequence.StartClock = rndm.Next(2, sequence.MaxClock);          
            string[] brokenNumbers = new string[2] { "", "" };
            for (int i = 0; i < brokenNumbers.Length; i++)
            {
                if (rndm.Next(2) == 1)
                {
                    for (int a = 0; a < 7; a++)
                    {
                        if (rndm.Next(3) == 1)
                            brokenNumbers[i] += "1";
                        else
                            brokenNumbers[i] += "0";
                    }
                }
                else
                    brokenNumbers[i] = "0000000";
            }
            sequence.BrokenNumbers = brokenNumbers;
            return sequence;
        }
        public int[] GetClosestValuesFromArray(int[] values, string[] clock)
        {
            List<int> returnValues = new List<int>();
            for (int i = 0; i < values.Length; i++)
            {
                string[] valueClock = ConvertClocks(values[i]);
                bool truth = true;
                for (int a = 0; a < valueClock.Length; a++)
                {
                    for (int b = 0; b < valueClock[a].Length; b++)
                    {
                        if (!IsSectionBrokenOrEqual(valueClock[a][b], clock[a][b]))
                            truth = false;
                    }                    
                }
                if (truth)
                    returnValues.Add(ToInt(valueClock));
            }
            return returnValues.OrderBy(v => v).ToArray();
        }
        public bool IsSectionBrokenOrEqual(char realSeq, char userSeq)
        {
            if((realSeq == '1' && userSeq == '0') || (realSeq == userSeq))
                return true;
            return false;
        }
        public int[] GetClosestValues(string[] clock)
        {
            List<List<string>> values = new List<List<string>>();
            values.Add(new List<string>());
            values.Add(new List<string>());
            for (int i = 0; i < clock.Length; i++)
            {
                for (int a = 0; a < clocks.Length; a++)
                {
                    bool truth = true;
                    for (int b = 0; b < clock[i].Length; b++)
                    {
                        if(!IsSectionBrokenOrEqual(clocks[a][b], clock[i][b]))
                        {
                            truth = false;
                            break;
                        }
                    }
                    if (truth && !(clocks[a] == "1110111" && i == 1))
                        values[i].Add(clocks[a]);
                }
            }
            List<int> returnValues = new List<int>();
            for (int i = 0; i < values[0].Count; i++)
            {
                for (int a = 0; a < values[1].Count; a++)
                {
                    string[] newClock = new string[2];
                    newClock[0] = values[0][i];
                    newClock[1] = values[1][a];
                    returnValues.Add(ToInt(newClock));
                }
            }
            return returnValues.OrderBy(v => v).ToArray();
        }
        public int GetIndexOfClock(string num)
        {
            for (int i = 0; i < clocks.Length; i++)
            {
                if (clocks[i] == num)
                {
                    return i;
                }
            }
            return 404;
        }
        public string GetSwitchedColor(string color)
        {
            switch (color)
            {
                case "green":
                    return "red";                   
                case "red":
                    return "green";
                default:
                    return null;
            }
        }
        public TrafficLight ConvertToTrafficLight(Sequence sequence, Observation preObservation)
        {
            TrafficLight trafficLight = new TrafficLight();
            int preClockInt = ToInt(preObservation.Numbers);
            int nowClockInt = preClockInt - 1;
            if (nowClockInt < 1)
            {
                trafficLight.Color = GetSwitchedColor(preObservation.Color);
                nowClockInt = sequence.MaxClock;
            }
            else
                trafficLight.Color = preObservation.Color;
            if (trafficLight.Color == "green")
                trafficLight.Clock = GetBrokenClock(ConvertClocks(nowClockInt), sequence.BrokenNumbers);
            return trafficLight;
        }
        public string[] GetCurrentClockFromPre(string[] preClock)
        {
            int clockInt = ToInt(preClock);
            if (clockInt == 1)
                return null;
            return ConvertClocks(clockInt - 1);
        }
        public bool GetTrueClockValide(string[] userClock, string[] brokenSections, string[] realClock)
        {
            for (int i = 0; i < userClock.Length; i++)
            {
                for (int a = 0; i < userClock.Length; i++)
                {
                    if ((userClock[i][a] == '0' && brokenSections[i][a] == '0' && realClock[i][a] == '1') ||
                        (userClock[i][a] == '1' && brokenSections[i][a] == '1' && realClock[i][a] == '1') ||
                        (userClock[i][a] == '1' && brokenSections[i][a] == '1' && realClock[i][a] == '0'))
                        return false;
                }
            }          
            return true;
        }
        public string GetClockFromList(char[] sections)
        {
            string clock = "";
            for (int i = 0; i < sections.Length; i++)
            {
                clock += sections[i];
            }
            return clock;
        }
        public string[] GetBrokenClock(string[] clock, string[] missingSections)
        {
            string[] returnClocks = new string[2] { "", "" };
            for (int i = 0; i < clock.Length; i++)
            {
                for (int a = 0; a < clock[i].Length; a++)
                {
                    if (clock[i][a] == missingSections[i][a] || (clock[i][a] == '0' && missingSections[i][a] == '1'))
                        returnClocks[i] += '0';
                    else
                        returnClocks[i] += '1';
                }
            }
            return returnClocks;
        }
        public void SetObservationClocks(ref Observation newObservation, ref Sequence sequence, string[] curClocks, string[] curUserClocks)
        {
            string[] newNickedNums = new string[2];
            for (int i = 0; i < curUserClocks.Length; i++)
            {
                string num = GetMissingSections(curUserClocks[i],
                    GetIndexOfClock(curClocks[i]),
                    sequence.BrokenNumbers[i],
                    sequence.NickedBrokenNumbers[i]
                    );
                newNickedNums[i] = num;
                newObservation.Numbers[i] = curClocks[i];
            }
            sequence.NickedBrokenNumbers = newNickedNums;
            newObservation.NumbersStr = $"{curClocks[0]} {curClocks[1]}";
            string newId = newObservation.Color;
            for (int i = 0; i < newObservation.Numbers.Length; i++)
            {
                newId += newObservation.Numbers[i];
            }
            newObservation.Id = newId;
        }
        public string[] ConvertClocks(int clocksInt)
        {
            string clocksStr = clocksInt.ToString();
            if (clocksStr.Length == 1)
                clocksStr = '0' + clocksStr;
            string[] returnClocks = new string[clocksStr.Length];
            for (int i = 0; i < clocksStr.Length; i++)
            {
                returnClocks[i] = clocks[Convert.ToInt32(clocksStr[i].ToString())];
            }
            return returnClocks;
        }
        public string GetMissingSections(string curUserClock, int curClockInt, string missingClocks, string nickedMissingClocks)
        {
            string curClock = clocks[curClockInt];
            char[] returnClock = nickedMissingClocks.ToCharArray();
            for (int i = 0; i < curClock.Length; i++)
            {
                if ((curUserClock[i] == '1' && curClock[i] == '0') ||
                    (curUserClock[i] == '0' && curClock[i] == '1' && missingClocks[i] == '0'))
                    return null;
                else if (curUserClock[i] == '0' && curClock[i] == '1' && missingClocks[i] == '1')
                    returnClock[i] = '1';
            }
            return GetClockFromList(returnClock);
        }
        public int ToInt(string[] clock)
        {
            string clockStr = GetIndexOfClock(clock[0]).ToString();
            clockStr += GetIndexOfClock(clock[1]).ToString();
            return Convert.ToInt32(clockStr);
        }
    }
}
