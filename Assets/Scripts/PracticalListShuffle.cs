using System.Collections.Generic;
using UnityEngine;

namespace GeneralScript {
    public static class PracticalListShuffle {
        public static List<T> FisherYatesShuffle<T>(List<T> argList) {
            if(argList == null) {
                return null;
            }

            List<T> tempList = new List<T>(argList);

            for(int i = tempList.Count - 1; i > 0; --i) {
                int j = Random.Range(0, i + 1);

                (tempList[j], tempList[i]) = (tempList[i], tempList[j]);
            }

            return tempList;
        }
    }
}
