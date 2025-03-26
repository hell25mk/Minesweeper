using System.Collections.Generic;
using UnityEngine;

namespace GeneralScript {
    public static class UniqueRandomGenerator {
        public static List<int> UniqueRandomInt(int begin, int stop, int count) {
            int size = stop - begin;

            if(begin >= stop || count > size) {
                return new List<int>();
            }

            // ”š—Ê‚É‘Î‚µ‚Ä¶¬”‚ª70%ˆÈ‰º‚Ìê‡‚ÍHashSet
            if((float)(count / size) * 100 < 60.0f) {
                return GenerateHashSet(begin, stop, count);
            }
            else {
                return GenerateList(begin, stop, count);
            }
        }

        private static List<int> GenerateHashSet(int min, int max, int count) {
            HashSet<int> result = new HashSet<int>();

            while(result.Count < count) {
                int rand = Random.Range(min, max);
                result.Add(rand);
            }

            return new List<int>(result);
        }

        private static List<int> GenerateList(int min, int max, int count) {
            List<int> result = new List<int>();

            for(int i = min; i < max; i++) {
                result.Add(i);
            }

            result = PracticalListShuffle.FisherYatesShuffle<int>(result);

            return result.GetRange(0, count);
        }
    }
}
