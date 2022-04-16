using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JitOptimize
{
    /// Jit最適化コンパイルによる定数の畳み込みを確認します
    /// 
    /// <summary>
    /// [最適化無効 jitコンパイル後]
    ///     25:         static void Main(string[] args)
    ///    26:         {
    /// push ebp
    /// mov ebp, esp
    /// push edi
    /// push esi
    /// push ebx
    /// sub esp,40h
    /// mov         esi,ecx
    /// lea         edi,[ebp-4Ch]
    ///    mov ecx,10h
    /// xor         eax,eax
    /// rep stos dword ptr es:[edi]
    ///    mov ecx, esi
    ///  mov dword ptr[ebp - 3Ch],ecx
    /// cmp         dword ptr ds:[2E542F0h],0  
    /// je JitOptimize.Program.Main(System.String[])+02Ah(02EE0872h)
    ///  call        61D805E0  
    ///  xor edx, edx
    ///  mov dword ptr[ebp - 40h],edx
    /// xor         edx,edx
    /// mov         dword ptr[ebp - 44h], edx
    /// nop  
    ///    27:             int i = 4;
    ///    mov dword ptr[ebp - 40h],4  
    ///    28:             int j = 3 * i + 11;
    ///    mov eax, dword ptr[ebp - 40h]
    /// lea         eax,[eax+eax*2]
    ///    add eax,0Bh
    /// mov         dword ptr[ebp - 44h], eax
    ///    29:             Console.WriteLine(Add(i, j));
    /// mov ecx, dword ptr[ebp - 40h]
    /// mov         edx,dword ptr[ebp - 44h]
    /// call JitOptimize.Program.Add(Int32, Int32) (02EE0438h)  
    /// mov dword ptr[ebp - 48h],eax
    /// mov         ecx,dword ptr[ebp - 48h]
    /// call System.Console.WriteLine(Int32) (61151938h)  
    /// nop  
    ///    30:             Console.ReadLine();
    /// call System.Console.ReadLine() (61151830h)  
    /// 
    /// [最適化有効 jitコンパイル後]
    ///  push        ebp  
    ///  mov ebp, esp
    ///  mov ecx,1Bh     ←★定数27が直接埋め込まれる
    /// call        System.Console.WriteLine(Int32) (61151938h)  
    /// 20:             Console.ReadLine();
    /// call System.Console.ReadLine() (61151830h)  
    /// </summary>
    internal class Program
    {
        static int Add(int i, int j)
        {
            return i + j;
        }
        static void Main(string[] args)
        {
            int i = 4;
            int j = 3 * i + 11;
            Console.WriteLine(Add(i, j));
            Console.ReadLine();
        }
    }
}
