/*
 * Intel(R) Core(TM) i7-3520M CPU @ 2.90GHz   2.90 GHz
 * Cache Line Size  = 64
 * L2 Associativity = 6
 * Cache Size       = 256 [Kb]
 */
using System.Runtime.Intrinsics.X86;
int ecx, nExIds;
uint id = (uint)0x80000006;

(nExIds, _, _, _) = X86Base.CpuId(int.MinValue, 0);
if ((uint)nExIds >= id)
{
    (_, _, ecx, _) = X86Base.CpuId((int)id, 0);
    var nCacheLineSize = ecx & 0xff;
    var nL2Associativity = (ecx >> 12) & 0xf;
    var nCacheSizeK = (ecx >> 16) & 0xffff;

    Console.WriteLine("Cache Line Size  = {0}", nCacheLineSize);
    Console.WriteLine("L2 Associativity = {0}", nL2Associativity);
    Console.WriteLine("Cache Size       = {0} [Kb]", nCacheSizeK);
}
