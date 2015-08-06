using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Common
{
   public static class AudioHelper
   {
       /// 播放 .wav 格式的声音文件    
       /// </summary>    
       /// <param name="location">声音文件路径 </param>    
       public static void Play(string location);

       /// <summary>    
       /// 播放 .wav 格式的声音文件    
       /// </summary>    
       /// <param name="playMode">播放声音的枚举模式。默认为AudioPlayMode.Background。</param>    
       /// <param name="location">声音文件路径</param>    
       public static void Play(string location, AudioPlayMode playMode);

       /// <summary>    
       /// 播放 .wav 格式的声音文件    
       /// </summary>    
       /// <param name="stream"><see cref="T:System.IO.Stream"></see>声音文件的流对象</param>    
       /// <param name="playMode">播放声音的枚举模式。默认为AudioPlayMode.Background。</param>    
       public static void Play(Stream stream, AudioPlayMode playMode);

       /// <summary>    
       /// 播放 .wav 格式的声音文件    
       /// </summary>    
       /// <param name="data">声音文件的字节数组</param>    
       /// <param name="playMode">播放声音的枚举模式。默认为AudioPlayMode.Background。</param>    
       public static void Play(byte[] data, AudioPlayMode playMode);

       /// <summary>    
       /// 播放系统声音    
       /// </summary>    
       public static void PlaySystemSound(SystemSound systemSound);

       /// <summary>    
       /// 停止正在后台播放的声音    
       /// </summary>    
       public static void Stop();
   }
}
