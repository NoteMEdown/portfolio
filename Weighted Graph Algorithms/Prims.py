# -*- coding: utf-8 -*-
# @author Nathan Frazier
from functions import *

def prims(wG, showSteps, startVertex = 0): # Take a graph object and which vertex to start at. 0 by default
    T = ({startVertex}, []) # Create a tuple with a set consisting of only the starting vertex, 
                            # and an empty list of edges
    while T[0] != wG.vertex_set(): # While the vertex set of the graph and subgraph are not equal
        update(T, wG) # Update the subgraph by adding the correct edge to it
        if showSteps:
            wG.draw_subgraph(T); # Shows each step
    print(f"Cost of graph after using Prims: {sum([cost(e, wG) for e in T[1]])}") # Print total cost of edges in subgraph
    return T # Return the subgraph