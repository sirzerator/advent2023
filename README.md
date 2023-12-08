# Advent 2023 in C\#

An opportunity to practice a new language, add a string to my bow.
Why C#? I'm starting a contract in C#, so that's a way to kill two birds with
one stone, in a way.

## Day 1

Got stuck way too long on the overlapping matches. For example, "fiveight" maps
to "58" and "twone" to "21", but I incorrectly assumed, because of the
examples, that it mapped to "5ight" and "2ne".

## Day 2

This day was very straightforward. However, my first iteration was way too
slow. It appears that recompiling your regular expressions in a loop is highly
inefficient... Just moving them to static attributes makes it faster. It's
still a bit slow but I might come back to it when I learn of a better way.

## Day 3

Took some time to fix all corner cases, but it ended up working.
