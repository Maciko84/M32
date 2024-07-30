.data ; the length of data is 1048 ints (each adress = one int)
char 'h' ; 0
char 'i' ; 1
char '.' ; 2
char 't' ; 3
char 'x' ; 4
.text

; hi.txt\0
push 0
push 3
load
push 4
load
push 3
load
push 2
load
push 1
load
push 0
load ; pops a value from the stack and pushes a value from the memory with this adress

; fopen
push 5
custom ; like a syscall but in m32

; store the fd at #5 of the data memory
push 5
store 

; write hi
push 0
load
push 5
load
push 6
custom ; #6 - fprintc

push 1
load
push 5
load
push 6
custom

push 5
load
push 11
custom ; #11 - fclose
