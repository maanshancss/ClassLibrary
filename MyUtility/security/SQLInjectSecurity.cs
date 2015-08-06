//======================================================================
//
//        Copyright (C) 2008-2009 Ҽ������    
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
        /// ��̬���������_sword�Ƿ����SQL�ؼ���
        /// </summary>
        /// <param name="_sWord">�������ַ���</param>
        /// <returns>����SQL�ؼ��ַ���true�������ڷ���false</returns>
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
 