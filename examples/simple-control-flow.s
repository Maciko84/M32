; simple control flow
.data
char 'I' ; #0
 
.text
; initialize a counter
push 10
push 1
store


; print I
start:
push 0
load
push 3
custom

; decrement the counter
push 1
load
push 1
sub
push 1
store

; jump to the beginning if the counter is 0

push start

push 1
load
push 0
equ
not

cjmp