using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JitOptimizeRangeCheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            uint[] array = new uint[100];
            // No1. 範囲チェックなし
            //for (int k = 0; k < array.Length; ++k)
            //{
            //    array[k] = (uint)k;
            //}
            // No2 範囲チェックあり
            for (int k = 0; k < array.Length / 2; k++)
            {
                array[k * 2] = (uint)k;
            }
        }
        //No1. ■範囲チェックなし
        //026C0848 push        ebp  
        //026C0849 mov         ebp,esp  
        //026C084B push        esi  
        //026C084C mov         ecx,609A7F92h  　　　　　　　　　　 配列要素の型
        //026C0851 mov         edx,64h  　　　　　　　　　　　　　 配列サイズ
        //026C0856 call        CORINFO_HELP_NEWARR_1_VC(0AE322Ch)  配列の作成
        //026C085B mov         esi,eax  
        //    14:             
        //    15:             for (int k = 0; k<array.Length; ++k)
        //026C085D xor         edx,edx  
        //    15:             for (int k = 0; k<array.Length; ++k)
        //026C085F mov         eax,dword ptr[esi + 4]  　　　　　　esi=配列 eax=配列長
        //026C0862 test        eax,eax  　　　　　　　　　　　　　　　　　　　　　　　　　　　　　 配列が空ならば
        //026C0864 jle         JitOptimizeRangeCheck.Program.Main(System.String[])+027h(026C086Fh) ループしない
        //    16:             {
        //    17:                 array[k] = (uint) k;
        //026C0866 mov         dword ptr[esi + edx * 4 + 8], edx  　　　　　　　　　　　　　　　　　配列[k] = k
        //    15:             for (int k = 0; k<array.Length; ++k)
        //026C086A inc         edx  　　　　　　　　　　　　　　　                                  ++k
        //026C086B cmp         eax,edx  　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　配列長>kの場合のみ
        //026C086D jg          JitOptimizeRangeCheck.Program.Main(System.String[])+01Eh(026C0866h)　次の繰り返しに戻る
        //026C086F pop         esi  
        //026C0870 pop         ebp  
        //026C0871 ret

        //No2. ■範囲チェックあり
        //01680848  push ebp  
        //01680849  mov ebp, esp  
        //0168084B push        edi  
        //0168084C push        esi  
        //0168084D  push ebx  
        //0168084E  mov ecx,609A7F92h  
        //01680853  mov edx,64h  
        //01680858  call CORINFO_HELP_NEWARR_1_VC(0151322Ch)
        //0168085D  mov edi, eax  
        //    24:             // No2 範囲チェックあり
        //    25:             for (int k=0;k<array.Length/2;k++)
        //0168085F  xor edx, edx  
        //    25:             for (int k=0;k<array.Length/2;k++)
        //01680861  mov ebx, dword ptr[edi + 4]  
        //01680864  sar ebx,1  //除算
        //01680866  jns JitOptimizeRangeCheck.Program.Main(System.String[])+023h(0168086Bh)
        //01680868  adc ebx,0  
        //0168086B test        ebx,ebx  
        //0168086D  jle JitOptimizeRangeCheck.Program.Main(System.String[])+03Bh(01680883h)
        //0168086F  mov esi, dword ptr[edi + 4]  
        //    26:             {
        //    27:                 array[k * 2] = (uint) k;
        //01680872  mov eax, edx  
        //01680874  add eax, eax  
        //01680876  cmp eax, esi  
        //01680878  jae JitOptimizeRangeCheck.Program.Main(System.String[])+040h(01680888h) //配列の範囲チェック
        //0168087A mov         dword ptr[edi + eax * 4 + 8], edx  
        //    25:             for (int k=0;k<array.Length/2;k++)
        //0168087E  inc edx  
        //0168087F  cmp ebx, edx  
        //01680881  jg JitOptimizeRangeCheck.Program.Main(System.String[])+02Ah(01680872h)
        //01680883  pop ebx  
        //01680884  pop esi  
        //01680885  pop edi  
        //01680886  pop ebp  
        //01680887  ret  
        //01680888  call        6215F1B0  
        //0168088D  int         3  
    }
}
