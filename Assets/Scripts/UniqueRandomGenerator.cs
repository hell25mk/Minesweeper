using System.Collections.Generic;
using UnityEngine;

namespace GeneralScript {
    public static class UniqueRandomGenerator {
        /// <summary>
        /// 重複しない乱数のリストを返す
        /// 除外値の数によってはmaxCountに満たない場合がある
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値</param>
        /// <param name="maxCount">最大生成量</param>
        /// <param name="excludes">除外値</param>
        /// <returns>乱数リスト</returns>
        public static List<int> UniqueRandomInt(int minInclusive, int maxExclusive, int maxCount, List<int> excludes = null) {
            int size = maxExclusive - minInclusive;

            if(minInclusive >= maxExclusive || maxCount > size) {
                return new List<int>();
            }

            HashSet<int> excludeSet = new HashSet<int>(excludes) ?? new HashSet<int>();

            // 数字量に対して生成数が70%以下の場合はHashSet
            if((float)(maxCount / size) * 100 < 60.0f) {
                return GenerateHashSet(minInclusive, maxExclusive, maxCount, excludeSet);
            }
            else {
                return GenerateList(minInclusive, maxExclusive, maxCount, excludeSet);
            }
        }

        /// <summary>
        /// HashSetで乱数リストを生成し返す
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値</param>
        /// <param name="maxCount">最大生成量</param>
        /// <param name="excludeSet">除外値</param>
        /// <returns>乱数リスト</returns>
        private static List<int> GenerateHashSet(int minInclusive, int maxExclusive, int maxCount, HashSet<int> excludeSet) {
            HashSet<int> result = new HashSet<int>();

            while(result.Count < maxCount) {
                int rand = Random.Range(minInclusive, maxExclusive);

                if(!excludeSet.Contains(rand)) {
                    result.Add(rand);
                }
            }

            return new List<int>(result);
        }

        /// <summary>
        /// Listをシャッフルし、必要数を取り出したリストを返す
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値</param>
        /// <param name="maxCount">最大生成量</param>
        /// <param name="excludeSet">除外値</param>
        /// <returns>List<int></returns>
        private static List<int> GenerateList(int minInclusive, int maxExclusive, int maxCount, HashSet<int> excludeSet) {
            List<int> result = new List<int>();

            for(int i = minInclusive; i < maxExclusive; i++) {
                if(!excludeSet.Contains(i)) {
                    result.Add(i);
                }
            }

            result = PracticalListShuffle.FisherYatesShuffle(result);

            return result.GetRange(0, Mathf.Min(maxCount, result.Count));
        }
    }
}
