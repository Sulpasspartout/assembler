using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assembler_console
{
    class Program
    {
        static void Main(string[] args)
        {
            assembler a = new assembler();
            //heloo
            for (int i = 0; i < 6; i++)
            {
                command c = new command();
                c.label = Console.ReadLine();
                c.comnd = Console.ReadLine();
                c.operand = Console.ReadLine();
                if (!c.label.Equals(""))
                    a.driver(c.label, c.comnd, c.operand);
                else
                    a.driver(null, c.comnd, c.operand);
                global.line++;
            }
            writerecord r = new writerecord();
            r.write(a.obs);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"objcode", false))
            {
                foreach (record x in r.records)
                {
                    file.Write("T{0:x5}",x.address);
                    file.Write("{0:x2}",x.size);
                    foreach (obcode o in x.rec)
                    {
                        if (o.type == 0)
                        {
                            if (o.operand != null)
                            {
                                if (a.symtable.ContainsKey(o.operand))
                                {
                                    if (o.index)
                                        file.Write("{0:x6}", o.opcode * 0x10000 + 0x8000 + a.symtable[o.operand]);
                                    else
                                        file.Write("{0:x6}", o.opcode * 0x10000 + a.symtable[o.operand]);
                                }
                            }
                            else
                            {
                                if (o.index)
                                    file.Write("{0:x6}", o.opcode * 0x10000 + 0x8000 + o.operandcode);
                                else
                                    file.Write("{0:x6}", o.opcode * 0x10000 + o.operandcode);
                            }
                            
                        }
                        else if (o.type == 3)
                        {
                            file.Write("{0:x6}", o.numericaloperand);
                        }
                        else if (o.type == 40)
                        {
                            string s = "{0:x";
                            s += o.size.ToString() + "}";
                            file.Write(s, o.badbigfatoperand);
                        }
                        else if (o.type == 41)
                        {
                            foreach (char c in o.bytes)
                                file.Write("{0:x2}", (int)c);
                        }
                        else if (o.type == 6)
                        {
                            file.Write("4c0000");
                        }

                    }
                    file.WriteLine();
                }
            }
            
            Console.ReadLine();
        }
    }
}
