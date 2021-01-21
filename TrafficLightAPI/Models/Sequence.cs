using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficLightAPI.Models
{
    public class Sequence
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StartColor { get; set; } = "green";
        public string BrokenNumbersStr { get; set; } = "0000000 1000010";
        public string NickedBrokenNumbersStr { get; set; } = "0000000 0000000";
        public string StartClocksStr { get; set; } 
        public bool IsOver { get; set; }
        public int StartClock { get; set; } = 2;
        public DateTime CatheringDate { get; set; }
        public int MaxClock { get; set; } = 2;
        [NotMapped]
        public string[] BrokenNumbers
        {
            get
            {
                return BrokenNumbersStr.Split(new char[] { ' ' });
            }
            set
            {
                BrokenNumbersStr = $"{value[0]} {value[1]}";
            }
        }
        [NotMapped]
        public string[] NickedBrokenNumbers
        {
            get
            {
                return NickedBrokenNumbersStr.Split(new char[] { ' ' });
            }
            set
            {
                NickedBrokenNumbersStr = $"{value[0]} {value[1]}";
            }
        }
        [NotMapped]
        public int[] StartClocks
        {
            get
            {
                string[] startClocksStr = StartClocksStr.Split(new char[] { ' ' });
                int[] startClocksInt = new int[startClocksStr.Length];
                for (int i = 0; i < startClocksStr.Length; i++)
                {
                    startClocksInt[i] = Convert.ToInt32(startClocksStr[i]);
                }
                return startClocksInt;
            }
            set
            {
                StartClocksStr = "";
                for (int i = 0; i < value.Length; i++)
                {
                    StartClocksStr += value[i] + " ";
                }
                StartClocksStr = StartClocksStr.Trim();
            }
        }
        public Sequence()
        {
            DateTime now = DateTime.Now;
            CatheringDate.AddMilliseconds(now.Millisecond);
        }
    }
}
