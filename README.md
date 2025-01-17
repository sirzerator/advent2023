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

## Day 4

As is usual, my first solution was basically the brute-force approach. It was
fine for the first part because every line had to be computed at least once,
but for the second part, it really didn't cut it... Because of the
combinatorial explosion, we had to avoid any useless or repeated work. I used
memoization, that is saving the first results in a Dictionary (HashMap of
sorts) in order to avoid redoing the heavy work everytime and just work with
integers and dictionary queries.

## Day 5

I had to use threads to finish this one in reasonable time on my i5-3570K... It
was a nice opportunity to try the async..await features of C#. I also had an
off-by-one error in a loop that threw me off way too long on part 2. The
iteration time being so long, it took some time before I noticed the problem.

## Day 6

I started working on an algebraic solution and rapidly zeroed-in on a quadratic
equation. Solving was straightforward from there, especially for Part 2. I'm
getting the impression that the problems' author is trying to hit the
combinatorial explosion of naive, brute-force solutions to force us to work
around that.
