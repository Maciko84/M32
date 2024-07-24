; Hello World Program
.data
char 'H'   ; #0 - ASCII value 72
char 'e'   ; #1 - ASCII value 101
char 'l'   ; #2 - ASCII value 108
char 'l'   ; #3 - ASCII value 108
char 'o'   ; #4 - ASCII value 111
char ','   ; #5 - ASCII value 44
number 32  ; #6 - ASCII value for space ' '
char 'W'   ; #7 - ASCII value 87
char 'o'   ; #8 - ASCII value 111
char 'r'   ; #9 - ASCII value 114
char 'l'   ; #10 - ASCII value 108
char 'd'   ; #11 - ASCII value 100
char '!'   ; #12 - ASCII value 33
number 10  ; #13 - ASCII value for newline '\n'

.text

; Print "Hello, World!\n"
; Push each character address onto the stack and print it

push 0        ; Push address of 'H'
load          ; Load 'H'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'H'

push 1        ; Push address of 'e'
load          ; Load 'e'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'e'

push 2        ; Push address of 'l'
load          ; Load 'l'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'l'

push 3        ; Push address of 'l'
load          ; Load 'l'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'l'

push 4        ; Push address of 'o'
load          ; Load 'o'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'o'

push 5        ; Push address of ','
load          ; Load ','
push 3        ; Push custom instruction ID for printing a character
custom        ; Print ','

push 6        ; Push address of space (ASCII value 32)
load          ; Load space character
push 3        ; Push custom instruction ID for printing a character
custom        ; Print ' '

push 7        ; Push address of 'W'
load          ; Load 'W'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'W'

push 8        ; Push address of 'o'
load          ; Load 'o'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'o'

push 9        ; Push address of 'r'
load          ; Load 'r'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'r'

push 10       ; Push address of 'l'
load          ; Load 'l'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'l'

push 11       ; Push address of 'd'
load          ; Load 'd'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print 'd'

push 12       ; Push address of '!'
load          ; Load '!'
push 3        ; Push custom instruction ID for printing a character
custom        ; Print '!'

; End of Program
nop ; No operation
