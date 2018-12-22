# -*- coding: utf-8 -*-
# @author Nathan Frazier
from Prims import prims
from Kruskals import kruskals
from Weighted_Graph import Weighted_Graph

wG = Weighted_Graph('test_graph.txt') # Make a Weighted_Graph object based on the text file


def main():  # Asks user to input either prims or kruskals algorithm that they want to use.
    askUser = input('Please type prims or kruskals, for whichever algorithm you want to use: ')
    showSteps = input('Would you like to see each step of the algorith? Type yes or no: ') == 'yes'
    if askUser == 'prims':
        return wG.draw_subgraph(prims(wG, showSteps))  # Draws a graph using prims algorithm
    elif askUser == 'kruskals':
        return wG.draw_subgraph(kruskals(wG, showSteps)) # Draws a graph using kruskals algorithm
    else:
        print(f'Invalid syntax, please try again.') # Prints Invalid syntax if user didn't type prims or kruskals
    return

main()