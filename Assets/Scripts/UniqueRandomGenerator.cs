using System.Collections.Generic;
using UnityEngine;

namespace GeneralScript {
    public static class UniqueRandomGenerator {
        /// <summary>
        /// �d�����Ȃ������̃��X�g��Ԃ�
        /// ���O�l�̐��ɂ���Ă�maxCount�ɖ����Ȃ��ꍇ������
        /// </summary>
        /// <param name="minInclusive">�ŏ��l</param>
        /// <param name="maxExclusive">�ő�l</param>
        /// <param name="maxCount">�ő吶����</param>
        /// <param name="excludes">���O�l</param>
        /// <returns>�������X�g</returns>
        public static List<int> UniqueRandomInt(int minInclusive, int maxExclusive, int maxCount, List<int> excludes = null) {
            int size = maxExclusive - minInclusive;

            if(minInclusive >= maxExclusive || maxCount > size) {
                return new List<int>();
            }

            HashSet<int> excludeSet = new HashSet<int>(excludes) ?? new HashSet<int>();

            // �����ʂɑ΂��Đ�������70%�ȉ��̏ꍇ��HashSet
            if((float)(maxCount / size) * 100 < 60.0f) {
                return GenerateHashSet(minInclusive, maxExclusive, maxCount, excludeSet);
            }
            else {
                return GenerateList(minInclusive, maxExclusive, maxCount, excludeSet);
            }
        }

        /// <summary>
        /// HashSet�ŗ������X�g�𐶐����Ԃ�
        /// </summary>
        /// <param name="minInclusive">�ŏ��l</param>
        /// <param name="maxExclusive">�ő�l</param>
        /// <param name="maxCount">�ő吶����</param>
        /// <param name="excludeSet">���O�l</param>
        /// <returns>�������X�g</returns>
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
        /// List���V���b�t�����A�K�v�������o�������X�g��Ԃ�
        /// </summary>
        /// <param name="minInclusive">�ŏ��l</param>
        /// <param name="maxExclusive">�ő�l</param>
        /// <param name="maxCount">�ő吶����</param>
        /// <param name="excludeSet">���O�l</param>
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
