using System;
using System.Collections.Generic;
using System.IO;

namespace M32_CORE
{
    public class M32
    {
        private Stack<int> stack = new();
        private int[] mem;
        private Dictionary<int, Action<int[], Stack<int>>> customInstructions = new();

        public M32(int size = 1048)
        {
            mem = new int[size];
            
        }

        public void LoadAndRun(BinaryReader data)
        {
            var dataSectionLength = data.ReadUInt32();
            if (dataSectionLength > mem.Length)
            {
                throw new OverflowException("Data section too big for the memory!");
            }
            else
            {
                for (uint i = 0; i < dataSectionLength; i++)
                {
                    mem[i] = data.ReadInt32();
                }
            }

            run(data);
        }

        private void run(BinaryReader data)
        {
            // Read all instructions into a list
            List<byte> instructions = new();
            while (data.BaseStream.Position < data.BaseStream.Length)
            {
                instructions.Add(data.ReadByte());
            }

            Stack<int> callStack = new();

            int pc = 0; // program counter
            while (pc < instructions.Count)
            {
                var opcode = instructions[pc];
                pc++; // Move to the next instruction by default
                switch (opcode)
                {
                    case 0x00: // no-op
                        break;
                    case 0x01: // push
                        int pvalue = BitConverter.ToInt32(instructions.GetRange(pc, 4).ToArray(), 0);
                        stack.Push(pvalue);
                        pc += 4; // Move past the int32 value
                        break;
                    case 0x02: // add
                        PerformOperation((a, b) => a + b);
                        break;
                    case 0x03: // sub
                        PerformOperation((a, b) => a - b);
                        break;
                    case 0x04: // mul
                        PerformOperation((a, b) => a * b);
                        break;
                    case 0x05: // div
                        PerformOperation((a, b) => a / b);
                        break;
                    case 0x06: // mod
                        PerformOperation((a, b) => a % b);
                        break;
                    case 0x07: // and
                        PerformOperation((a, b) => a & b);
                        break;
                    case 0x08: // or
                        PerformOperation((a, b) => a | b);
                        break;
                    case 0x09: // xor
                        PerformOperation((a, b) => a ^ b);
                        break;
                    case 0x0A: // lls
                        PerformOperation((a, b) => a << b);
                        break;
                    case 0x0B: // rls
                        PerformOperation((a, b) => a >> b);
                        break;
                    case 0x0C: // not
                        if (stack.Count < 1)
                        {
                            throw new Exception("Stack underflow.");
                        }
                        else stack.Push(~stack.Pop());
                        break;
                    case 0x0D: // equ
                        PerformOperation((a, b) => Bool2Int(a == b));
                        break;
                    case 0x0E: // le
                        PerformOperation((a, b) => Bool2Int(a < b));
                        break;
                    case 0x0F: // ge
                        PerformOperation((a, b) => Bool2Int(a > b));
                        break;
                    case 0x10: // jmp
                        if (stack.Count < 1)
                        {
                            throw new Exception("Stack underflow.");
                        }
                        pc = stack.Pop();
                        break;
                    case 0x11: // cjmp
                        if (stack.Count < 2)
                        {
                            throw new Exception("Stack underflow.");

                        }
                        bool cjvalue = Int2Bool(stack.Pop());
                        int targetPc = stack.Pop();
                        if (cjvalue)
                        {
                            pc = targetPc;
                        }
                        break;
                    case 0x12: // store
                        if (stack.Count < 2)
                        {
                            throw new Exception("Stack underflow.");
                        }
                        int sadress = stack.Pop();
                        int svalue = stack.Pop();

                        if (sadress >= mem.Length)
                        {
                            throw new OverflowException("Adress too big!");
                        }

                        mem[sadress] = svalue;
                        break;
                    case 0x13: // load
                        if (stack.Count < 1)
                        {
                            throw new Exception("Stack underflow.");
                        }
                        int ladress = stack.Pop();

                        if (ladress >= mem.Length)
                        {
                            throw new OverflowException("Adress too big!");
                        }

                        stack.Push(mem[ladress]);
                        break;
                    case 0x14: // pop
                        stack.TryPop(out _);
                        break;

                    case 0x15: // custom
                        if(stack.Count < 1)
                        {
                            throw new Exception("Stack underflow.");
                        }
                        int id = stack.Pop();
                        if (!customInstructions.ContainsKey(id))
                        {
                            throw new Exception($"Cannot find custom operation #{id}!");
                        }
                        customInstructions[id](mem, stack);
                        break;

                    case 0x16: // call
                        if(stack.Count < 1)
                        {
                            throw new Exception("Stack underflow.");
                        }

                        callStack.Push(pc + 1);
                        pc = stack.Pop();
                        break;

                    case 0x17: // ret
                        if(callStack.Count > 1)
                        {
                            pc = callStack.Pop();
                        }
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown opcode: {opcode}");
                }
            }
        }

        private void PerformOperation(Func<int, int, int> operation)
        {
            if (stack.Count < 2)
            {
                throw new Exception("Stack Underflow.");
            }
            else
            {
                int b = stack.Pop();
                int a = stack.Pop();
                stack.Push(operation(a, b));
            }
        }

        private bool Int2Bool(int a) => (a & 1) == 1;

        private int Bool2Int(bool a) => a ? 1 : 0;

        public void Custom(int key, Action<int[], Stack<int>> action) {
            customInstructions[key] = action;
        }

    }
}
