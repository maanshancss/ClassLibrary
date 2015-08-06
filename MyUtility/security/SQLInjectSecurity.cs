//======================================================================
//
//        Copyright (C) 2008-2009 壹择网络    
//        All rights reserved
//
//        filename :SQLInjectSecurity
//        description :
//
//        Created by Conan at  2008-12-20 14:34:32
//        
//
//======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtility.security
{
    public class SQLInjectSecurity
    {
        private const string StrKeyWord = @"select|insert|delete|from|count(|drop table|update|truncate|asc(|mid(|char(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
        private const string StrRegex = @"[-|;|,|/|(|)|[|]|}|{|%|@|*|!|']";


        //// <summary>
        /// 静态方法，检查_sword是否包涵SQL关键字
        /// </summary>
        /// <param name="_sWord">被检查的字符串</param>
        /// <returns>存在SQL关键字返回true，不存在返回false</returns>
        public static bool CheckSQLKeyWord(string _sWord)
         {
            if (Regex.IsMatch(_sWord, StrKeyWord, RegexOptions.IgnoreCase) || Regex.IsMatch(_sWord, StrRegex))
                return true;
            return false;
        }


        public static bool HasKeywords(string contents)
        {
            bool bReturnValue = false;
            if (contents.Length > 0)
            {
                string sLowerStr = contents.ToLower();
                string sRxStr = @"(\sand\s)|(\sand\s)|(\slike\s)|(select\s)|(insert\s)|(delete\s)|(update\s[\s\S].*\sset)|(create\s)|(\stable)|(<[iframe|/iframe|script|/script])|(')|(\sexec)|(declare)|(\struncate)|(\smaster)|(\sbackup)|(\smid)|(\scount)|(cast)|(%)|(\sadd\s)|(\salter\s)|(\sdrop\s)|(\sfrom\s)|(\struncate\s)|(\sxp_cmdshell\s)";
                Regex sRx = new Regex(sRxStr);
                bReturnValue = sRx.IsMatch(sLowerStr, 0);
            }
            return bReturnValue;
        }

    }
}
 