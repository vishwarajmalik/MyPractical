using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAlgos
{
    class CLS_Algos
    {
        public void Init(string[] args)
        {

            string input = "Fri 05:00-10:00\n";

             int[] A = {6,1,4,6,3,2,7,4 };

            solution2(A, 3, 2);
        }

        // calculate sleep time

        public int solution2(int[] A, int K, int L)
        {

            // Implement your solution here

            Int32 maxSelect = 0;
            if ( (K + L )> A.Length)
            {
                maxSelect = - 1;
                return maxSelect;
            }
            else if ((K + L) == A.Length)
            {
                maxSelect = A.Sum();
                return maxSelect;
            }

            Dictionary<int, bool> dictTreeSelected = new Dictionary<int, bool>();

            int noOfTrees = A.Length;
            
            List<Int32> lstTotalApples = new List<Int32>();
            lstTotalApples.Add(0);

            for (int i = 0; i < noOfTrees; i++)
            {
                lstTotalApples.Add(lstTotalApples[i] + A[i]);
            }

            for (int i = 0; i <= noOfTrees - K; i++)
            {
                int person1 = lstTotalApples[i + K] - lstTotalApples[i];

                for (int j = 0; j <= noOfTrees - L; j++)
                {
                    if (j >= i + K || j + L <= i)
                    {
                        int person2 = lstTotalApples[j + L] - lstTotalApples[j];
                        maxSelect = Math.Max(maxSelect, person1 + person2);
                    }
                }

                dictTreeSelected[i] = true;
            }

            return maxSelect;
        }


        public int MaxApples(int[] A, int K, int L)
        {
            int n = A.Length;
            if (K + L > n) return -1;



            // Calculate the prefix sum of apples for efficient sliding window computation

            int maxApples = 0;
            List<Int32> lst = new List<Int32>();
            lst.Add(0);

            for (int i = 0; i < n; i++)
            {               
                lst.Add(lst[i] + A[i]);
            }
           
            for (int i = 0; i <= n - K; i++)
            {
                int person1 = lst[i + K] - lst[i];
               
                for (int j = 0; j <= n - L; j++)
                {
                    if (j >= i + K || j + L <= i)
                    {
                        int person2 = lst[j + L] - lst[j];
                        maxApples = Math.Max(maxApples, person1 + person2);
                    }
                }
            }

            return maxApples;
        }


        static int GetMaxApples(int[] apples)
        {
            int n = apples.Length;
            int maxApples = 0;

            for (int i = 0; i < n; i++)
            {
                int aliceApples = 0;
                int bobApples = 0;

                // Calculate apples for Alice
                for (int j = i; j < n; j += 2)
                {
                    aliceApples += apples[j];
                }

                // Calculate apples for Bob
                for (int j = i + 1; j < n; j += 2)
                {
                    bobApples += apples[j];
                }

                // Update maxApples if necessary
                maxApples = Math.Max(maxApples, Math.Max(aliceApples, bobApples));
            }

            return maxApples;
        }


        static int CalculateMaxApples(int[] apples, bool preferAlice)
        {
            int totalApples = 0;
            foreach (int apple in apples)
                totalApples += apple;

            int eachPersonGets = totalApples / 2;
            int remainingApples = totalApples % 2;

            if (preferAlice)
                return eachPersonGets + remainingApples;
            else
                return eachPersonGets;
        }

        public int Sleep_Solution(string S)
        {

            // Implement your solution here

            Int32 sleepDuration = 0;

            string[] arrinput = S.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<SlotBE> listSlot = new List<SlotBE>();

            DateTime minDate = DateTime.MinValue;

            foreach (var slot in arrinput)
            {

                Console.WriteLine($"slot row---> {slot}");
                try
                {
                    string[] parts = slot.Split(' ');
                    int noOfday = GetDayNoOfWeek(parts[0]);
                    string[] timeParts = parts[1].Split('-');
                    TimeSpan startTime = ParseTime(timeParts[0]);
                    TimeSpan endTime = ParseTime(timeParts[1]);

                    DateTime startDateTime = minDate.AddDays(noOfday).Add(startTime);
                    DateTime endDateTime = minDate.AddDays(noOfday).Add(endTime);

                    SlotBE slotBE = new SlotBE
                    {
                        StartTime = startDateTime,
                        EndTime = endDateTime,
                        SlotRow = slot
                    };
                    listSlot.Add(slotBE);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("error for " + slot + System.Environment.NewLine);
                    Console.WriteLine(ex.ToString());
                }
            }

            var listSlotSorted = listSlot.OrderBy(slot => slot.StartTime);

            DateTime prevEndTime = DateTime.MinValue;
            DateTime workEndTime = DateTime.MinValue.AddDays(6).Add(ParseTime("24:00"));

            foreach (SlotBE slot in listSlotSorted)
            {
                //Console.WriteLine($"chekcing for SlotRow {slot.SlotRow}");

                if (prevEndTime != DateTime.MinValue)
                {
                    TimeSpan gap = slot.StartTime - prevEndTime;

                    if (gap.TotalMinutes > sleepDuration)
                    {
                        sleepDuration = Convert.ToInt32(gap.TotalMinutes);
                        //Console.WriteLine($"sleepDuration -->  {sleepDuration} for slot {slot.SlotRow}");
                    }
                }
                else
                {
                    TimeSpan gap = slot.StartTime - prevEndTime;
                    if (gap.TotalMinutes > sleepDuration)
                    {
                        sleepDuration = Convert.ToInt32(gap.TotalMinutes);
                        //Console.WriteLine($"sleepDuration -->  {sleepDuration} for slot {slot.SlotRow}");
                    }
                }
                prevEndTime = slot.EndTime;
            }
            TimeSpan ts = workEndTime - prevEndTime;

            if (ts.TotalMinutes > sleepDuration)
            {
                sleepDuration = Convert.ToInt32(ts.TotalMinutes);

            }

            Console.WriteLine($"sleepDuration --> {sleepDuration}" );

            return sleepDuration;
        }

        static TimeSpan ParseTime(string timeString)
        {
            if (timeString == "24:00")
                return TimeSpan.FromHours(24);

            return TimeSpan.Parse(timeString);
        }

        public class SlotBE
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string SlotRow { get; set; }           
        }

        static int GetDayNoOfWeek(string day)
        {
            switch (day.ToLower())
            {
                case "sun":
                    return 6;
                case "mon":
                    return 0;
                case "tue":
                    return 1;
                case "wed":
                    return 2;
                case "thu":
                    return 3;
                case "fri":
                    return 4;
                case "sat":
                    return 5;
                default:
                    throw new ArgumentException("Invalid day of the week.");
            }
        }

        public static int test_solution(int[] A)
        {

            // Implement your solution here
            Int32 op = 1;
            List<Int32> lst = A.Distinct().ToList();
            lst.Sort();

            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i] != i + 1)
                {
                    op = i + 1;
                    break;
                }
            }
            Console.WriteLine(op);

            return op;

        }
    }
}
