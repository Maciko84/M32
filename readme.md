# Repository
The repository contains some binary builds for windows, examples and the source code for the core of the vm

# M32 Bytecode Instruction Set

The `M32` virtual machine uses a bytecode instruction set for its operations. This document outlines the instruction set and the corresponding assembly syntax used to generate the bytecode. 

## Instructions

The following instructions are available in the `M32` bytecode:

- **0x00** - **nop**  
  No operation. This is a placeholder instruction that does nothing.

- **0x01** - **push**  
  Pushes a 32-bit integer onto the stack.  
  *Syntax:*  
  `push <value>`  
  *Example:*  
  `push 42`  
  *Followed by:*  
  - 4 bytes of integer data (little-endian).

- **0x02** - **add**  
  Pops two values from the stack, adds them, and pushes the result.  
  *Syntax:*  
  `add`

- **0x03** - **sub**  
  Pops two values from the stack, subtracts the second from the first, and pushes the result.  
  *Syntax:*  
  `sub`

- **0x04** - **mul**  
  Pops two values from the stack, multiplies them, and pushes the result.  
  *Syntax:*  
  `mul`

- **0x05** - **div**  
  Pops two values from the stack, divides the first by the second, and pushes the result.  
  *Syntax:*  
  `div`  
  *Throws an exception if division by zero occurs.*

- **0x06** - **mod**  
  Pops two values from the stack, computes the modulus of the first value by the second, and pushes the result.  
  *Syntax:*  
  `mod`  
  *Throws an exception if division by zero occurs.*

- **0x07** - **and**  
  Pops two values from the stack, performs a bitwise AND operation, and pushes the result.  
  *Syntax:*  
  `and`

- **0x08** - **or**  
  Pops two values from the stack, performs a bitwise OR operation, and pushes the result.  
  *Syntax:*  
  `or`

- **0x09** - **xor**  
  Pops two values from the stack, performs a bitwise XOR operation, and pushes the result.  
  *Syntax:*  
  `xor`

- **0x0A** - **lls**  
  Pops two values from the stack, performs a left bitwise shift on the first value by the number of bits specified by the second value, and pushes the result.  
  *Syntax:*  
  `lls`

- **0x0B** - **rls**  
  Pops two values from the stack, performs a right bitwise shift on the first value by the number of bits specified by the second value, and pushes the result.  
  *Syntax:*  
  `rls`

- **0x0C** - **not**  
  Pops a value from the stack, performs a bitwise NOT operation, and pushes the result.  
  *Syntax:*  
  `not`

- **0x0D** - **equ**  
  Pops two values from the stack, compares them for equality, and pushes 1 if they are equal, or 0 if they are not.  
  *Syntax:*  
  `equ`

- **0x0E** - **le**  
  Pops two values from the stack, compares the first value to the second to check if the first is less than the second, and pushes 1 if true, or 0 if false.  
  *Syntax:*  
  `le`

- **0x0F** - **ge**  
  Pops two values from the stack, compares the first value to the second to check if the first is greater than the second, and pushes 1 if true, or 0 if false.  
  *Syntax:*  
  `ge`

- **0x10** - **jmp**  
  Pops a value from the stack and sets the program counter (PC) to this value.  
  *Syntax:*  
  `jmp`  
  *Example:*  
  `push 100`
  
  `jmp`

- **0x11** - **cjmp**  
  Pops two values from the stack. The first value is used as a condition, and the second value is used as the target address. If the condition value is non-zero, sets the program counter (PC) to the target address.  
  *Syntax:*  
  `cjmp`

- **0x12** - **store**  
  Pops two values from the stack: the first is the address and the second is the value. Stores the value in the specified memory address.  
  *Syntax:*  
  `store`

- **0x13** - **load**  
  Pops an address from the stack and pushes the value stored at that address in memory.  
  *Syntax:*  
  `load`

- **0x14** - **pop**  
  Pops a value from the stack and discards it.  
  *Syntax:*  
  `pop`

- **0x15** - **custom**  
  Pops an identifier from the stack and executes a custom instruction associated with that identifier.  
  *Syntax:*  
  `custom`

  (default customs are documented in the `Default Customs` section)

- **0x16** - **call**

  pops a adress from the stack and jumps to it, leaving the next instruction's adress on the call stack.  
  *Syntax:*

  `call`

- **0x17** - **ret**
  
  pops a adress from the call stack and jumps to it.

  *Syntax*: 
  `ret`

## Assembly Syntax

The assembler converts a human-readable assembly language into the bytecode format understood by the `M32` virtual machine. Here is the syntax for the assembly language supported by the `Assembler` class:

### Data Section

The data section defines data values to be loaded into memory. The data section starts with `.data` and includes the following:

- **`number <value>`**  
  Defines a 32-bit integer to be added to the data section.  
  *Example:*  
  `num 123`

- **`char '<char>'`**  
  Defines a single character (enclosed in single quotes) to be added to the data section. (still encoded as a 32-bit integer)

  *Example:*  
  `char 'A'`
  
  **note:** any whitespace must be defined using the `number` instruction followed by the ascii value.

### Text Section

The text section defines the bytecode instructions. The text section starts with `.text` and contains the instructions like in the syntax provided in the `Instructions` section of the document. Aslo it contains some extra things:
- `%set name integerValue` - declares a identifier with given value  
- `my_label:` - declares a label (works like %set my_label `<adress>`)

## Default Customs
The default runtime for the `m32` vm (the `m32` command) includes some simple customs for i/o:
- **printn (#1):**  
    Prints the top value of the stack as a number

- **inputn (#2):**  
    Inputs a number from the console and pushes it onto the stack

- **printc (#3):**  
    Prints the top value of the stack as a character

- **inputc (#4):**  
    Inputs a character from the console and pushes its ASCII value onto the stack

- **fopen (#5):**  
    Opens a file. Expects a string (file path) pushed character-by-character with a null terminator at the end of the string on the stack and returns a file descriptor

- **fprintc (#6):**  
    Prints a character to the file specified by the file descriptor on the stack

- **fprintn (#7):**  
    Prints a number to the file specified by the file descriptor on the stack

- **fclear (#8):**  
    Clears the contents of the file specified by the file descriptor on the stack

- **finputc (#9):**  
    Reads a specific character from the file specified by the file descriptor on the stack based on the index from the stack

- **flen (#10):**  
    Gets the length of the file specified by the file descriptor on the stack

- **fclose (#11):**  
    Closes the file specified by the file descriptor on the stack


## Notes

- **Stack Underflow:** Many instructions assume that the stack has a certain number of elements. Stack underflow exceptions will be thrown if there are insufficient elements on the stack.
- **Memory Addressing:** The `store` and `load` instructions use memory addresses, which must be within the bounds of the memory (default runtime's is `1048`).
- **Division and Modulus Operations:** Both `div` and `mod` instructions will throw exceptions if a division by zero is attempted.