using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// メソッドのインライン展開を確認します
/// </summary>
namespace JitOptimizeInlineExpansion
{
    public struct Point2D
    {
        public int X;
        public int Y;
    }

    internal class Program
    {
        private static void MethodThatTakesAPoint(Point2D pt)
        {
            pt.Y = pt.X ^ pt.Y;
            Console.WriteLine(pt.Y);
        }

        static void Main(string[] args)
        {
            Point2D pt;
            pt.X = 3;
            pt.Y = 5;
            MethodThatTakesAPoint(pt);
        }
        // [最適化無効]
        //        push ebx
        // sub esp,2Ch
        //xor         eax,eax
        //mov         dword ptr[ebp - 10h], eax
        // mov dword ptr[ebp - 1Ch],eax
        //cmp         dword ptr ds:[9942F0h],0  
        // je JitOptimizeInlineExpansion.Program.MethodThatTakesAPoint(JitOptimizeInlineExpansion.Point2D)+01Fh(0B308D7h)
        // call        61D805E0  
        // nop  
        //    19:             pt.Y = pt.X ^ pt.Y;
        // mov eax, dword ptr[ebp + 8]
        //lea         edx,[ebp+8]
        //        xor eax, dword ptr[edx + 4]
        // lea         edx,[ebp+8]
        //        mov dword ptr[edx + 4],eax  
        //    20:             Console.WriteLine(pt.Y);
        // lea eax,[ebp + 8]
        // mov ecx, dword ptr[eax + 4]
        //call        System.Console.WriteLine(Int32) (61151938h)  
        // nop  
        //    21:         }
        //    nop
        //    lea         esp,[ebp-0Ch]
        //    pop ebx
        // pop esi
        // pop edi
        // pop ebp
        // ret         8 

        // [最適化有効] .MethodThatTakesAPoinメソッドが呼ばれずにインライン展開されている
        //        push ebp
        // mov ebp, esp
        // mov eax, dword ptr[ebp + 8]
        //xor         dword ptr[ebp + 0Ch], eax  
        //    20:             Console.WriteLine(pt.Y);
        // mov ecx, dword ptr[ebp + 0Ch]
        //call        System.Console.WriteLine(Int32) (61151938h)  
        //pop ebp
        //ret         8  
    }
}
