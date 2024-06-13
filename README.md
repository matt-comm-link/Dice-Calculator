A tool for statistically estimating the probabilities of rolling a number of unique faces from two pools of dice, with the caveat that any duplicates in the second pool cancel out rolls in the first. 
Mostly made to get ballpark idea of how an idea for a tabletop game I'm cooking would actually work, but I imagine it has other use cases for other avoiding-the-birthday-paradox-but-sometimes-two-people-with-the-same-birthday-annihilate-each-other based board game ideas.

It's a very brute force solution to the problem and has a bare minimum amount of input sanitisation, but it works and that's enough for me. 
I'm sure there's a mathmatically beautiful way of calculating the exact probabilities of rolling dice like this but this program is not that.

Probably requires .Net 6.0
