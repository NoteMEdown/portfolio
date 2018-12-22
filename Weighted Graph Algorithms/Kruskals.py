# -*- coding: utf-8 -*-
# @author Nathan Frazier
from functions import *

def kruskals(wG, showSteps): # Take a graph object
    V = wG.vertex_set() # Vertex set of subgraph is the same as the vertex set of main graph
    sortedE = sortEdges(wG.edge_set(), wG) # Sorts all edges by weight
    E = [] # Make an empty list of Edges
    T = (V, E) # Make a tuple containing the set of vertices and list of edges
    index = 0 # Index starts at the lowest weighted edge
    while len(E) != (len(V) - 1): # While we do not have a single tree
        if checkCycleUnconnected(sortedE[index], T): # Checks if adding the lowest edge create a cycle
            sortedE.remove(sortedE[index]) # If a cycle is made, remove that edge from the list of possible edges
        else:
            E.append(sortedE[index]) # If no cycle is made, add the edge to the list of edges in the subgraph
            index += 1 # Increase Index
        if showSteps:
            wG.draw_subgraph(T); # Shows each step
    print(f"Cost of graph after using Kruskals: {sum([cost(e, wG) for e in T[1]])}") # Print total cost of edges in subgraph
    return T #return the subgraph