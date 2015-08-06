using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection;

namespace ESBasic.Helpers
{
    public static class NullReferenceHelper
    {
        public static string GetExceptionMethodAddress(Exception ee)
        {
            if (!(ee is NullReferenceException))
            {
                return "";
            }

            //获取调用堆栈
            StackTrace trace = new StackTrace(ee, true);
            StackFrame frame = trace.GetFrame(0);
            int offset = frame.GetILOffset();
            byte[] il = frame.GetMethod().GetMethodBody().GetILAsByteArray();

            //获取调用指令
            offset++;
            ushort instruction = il[offset++];
            //打开潘多拉魔盒
            ILGlobals global = new ILGlobals();
            

            //翻译
            OpCode code = OpCodes.Nop;
            if (instruction != 0xfe)
            {
                code = global.SingleByteOpCodes[(int)instruction];
            }
            else
            {
                instruction = il[offset++];
                code = global.MultiByteOpCodes[(int)instruction];
                instruction = (ushort)(instruction | 0xfe00);
            }

            //获取方法信息
            int metadataToken = NullReferenceHelper.ReadInt32(il, ref offset);
            MethodBase callmethod = frame.GetMethod().Module.ResolveMethod(metadataToken,
                 frame.GetMethod().DeclaringType.GetGenericArguments(),
                 frame.GetMethod().GetGenericArguments());
           
            return callmethod.DeclaringType + "." + callmethod.Name;  
        }

        #region ReadInt32
        private static int ReadInt32(byte[] il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        } 
        #endregion
    }

    public class ILGlobals
    {
        #region MultiByteOpCodes
        private OpCode[] multiByteOpCodes;
        public OpCode[] MultiByteOpCodes
        {
            get { return multiByteOpCodes; }
        }
        #endregion

        #region SingleByteOpCodes
        private OpCode[] singleByteOpCodes;
        public OpCode[] SingleByteOpCodes
        {
            get { return singleByteOpCodes; }
        }
        #endregion

        #region Ctor
        public ILGlobals()
        {
            singleByteOpCodes = new OpCode[0x100];
            multiByteOpCodes = new OpCode[0x100];
            FieldInfo[] infoArray1 = typeof(OpCodes).GetFields();
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                FieldInfo info1 = infoArray1[num1];
                if (info1.FieldType == typeof(OpCode))
                {
                    OpCode code1 = (OpCode)info1.GetValue(null);
                    ushort num2 = (ushort)code1.Value;
                    if (num2 < 0x100)
                    {
                        singleByteOpCodes[(int)num2] = code1;
                    }
                    else
                    {
                        if ((num2 & 0xff00) != 0xfe00)
                        {
                            throw new Exception("Invalid OpCode.");
                        }
                        multiByteOpCodes[num2 & 0xff] = code1;
                    }
                }
            }
        }
        #endregion

        #region ProcessSpecialTypes
        public static string ProcessSpecialTypes(string typeName)
        {
            string result = typeName;
            switch (typeName)
            {
                case "System.string":
                case "System.String":
                case "String":
                    result = "string"; break;
                case "System.Int32":
                case "Int":
                case "Int32":
                    result = "int"; break;
            }
            return result;
        }
        #endregion
    }
}
