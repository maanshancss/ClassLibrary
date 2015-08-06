using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Arithmetic
{
    /// <summary>
    /// 字串模糊匹配算法。
    /// </summary>
    public static class FuzzyMatchHelper
    {
        #region IsLike 嘟嘟杰作
        public static bool IsLike(string matchingStr, string targetStr)
        {
            string[] matchAry = matchingStr.Split('%');

            bool isContain = false;
            int startInSource = 0;
            for (int i = 0; i < matchAry.Length; i++)
            {
                if (matchAry[i] != "")
                {
                    int startIndex, endIndex;
                    isContain = FuzzyMatchHelper.ContainString(targetStr, startInSource, matchAry[i], out startIndex, out endIndex);
                    if (isContain)
                    {
                        if (i == 0 && startIndex != 0)
                        {
                            return false;
                        }
                        if (i == matchAry.Length - 1 && endIndex != targetStr.Length)
                        {
                            startInSource++;
                            i--;
                            continue;
                        }
                        startInSource = endIndex;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool ContainString(string sourceString, int startInSouce, string containString, out int startIndex, out int endIndex)
        {
            for (int i = startInSouce; i < sourceString.Length; i++)
            {
                if (sourceString[i] == containString[0] || containString[0] == '_')
                {
                    startIndex = i;
                    int j = 1;
                    for (; j < containString.Length; j++)
                    {
                        if (i + j >= sourceString.Length || (containString[j] != '_' && containString[j] != sourceString[startIndex + j]))
                        {
                            break;
                        }
                    }
                    if (j == containString.Length)
                    {
                        endIndex = startIndex + containString.Length;
                        return true;
                    }
                }
            }
            startIndex = -1;
            endIndex = -1;
            return false;
        }
        #endregion        
    }
}
