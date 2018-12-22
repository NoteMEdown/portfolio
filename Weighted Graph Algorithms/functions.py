# -*- coding: utf-8 -*-
# @author Nathan Frazier
def update(T, wG):
    edge = correctEdge(T, wG) # Find the correct edge to add the the subgraph
    T[0].add(edge[0]) # Add both of the vertices from the edge
    T[0].add(edge[1])
    T[1].append(edge) # Add the edge itself
    
def cost(edge, wG):
    return wG.edge_dict()[edge] # Returns the cost of a specific edge

def correctEdge(T, wG):
    E = [] # Create an empty list of edges
    for vertex in T[0]: # Go through every vertex in the subgraph
        for edge in wG.edge_set(): # Go through every edge in the main graph
            # If the edge is touching the vertex and is not already in our new list and not in our current subgraph
            if vertex in edge and edge not in E and edge not in T[1]: 
                E.append(edge) # Add this edge to our list of possible edges 
    removeCycles(E, T)  # Remove any edges that cause cycles          
    lowestWeightIndex = findLowestWeight(E, wG) # Choose the lowest weight out of remaining edges
    return E[lowestWeightIndex] # Return the lowest weight edge that causes no cycles
    
def findLowestWeight(E, wG):
    index = 0 # Set index to the first edge in the list of edges
    for i in range(1, len(E)):  # Go through each edge in the list
        if cost(E[index], wG) > cost(E[i], wG): # Compare the current smallest edge with current edge
            index = i # If its smaller, set the index to it's index
    return index # Return the index of the smallest weighted edge
        
def removeCycles(E, T):
    for i in range(len(E)): # Go through each edge in the list
        if checkCycle(E[len(E) - i - 1], T[0]): # Check if that edge causes a cycle
            E.remove(E[len(E) - i - 1]) # If it does, remove it
            
def checkCycle(edge, V):
    return edge[0] in V and edge[1] in V # return true if each vertex of this edge are already in the subgraph
            
# Below are functions for Kruskals

def sortEdges(E, wG): # Simple selection sort
    N = [e for e in wG.edge_set()]
    for i in range(len(N)-1):
        index = i
        for j in range(i+1, len(N)):
            if cost(N[j], wG) < cost(N[index], wG):
                index = j
        N[index], N[i] = N[i], N[index]   
    return N

def checkCycleUnconnected(edge, T):
    start = edge[0] # Start point is one side of the edge
    end = edge[1] # End point is the other side of the edge
    seen = [] # List of seen vertices (Vertices we have been to)
    relations = {} # Set of which vertices connect to which vertices
    
    for vertex in T[0]: # Go through each vertex in subgraph
        if vertex not in relations: # If the vertex is not in the set
            relations[vertex] = [] # Create an empty list for that vertex
        for edge in T[1]: # Go through each edges in subgraph
            if edge[0] == vertex: # If our current edge in on one side of the edge
                relations[vertex].append(edge[1]) # Add the other side
            elif edge[1] == vertex: # Same as above, yet checking for other side of edge
                relations[vertex].append(edge[0])
    # (If there is already a path, adding the edge would creata a cycle)
    return checkPath(start, end, seen, relations) # Check if there is already a path 
    
def checkPath(start, end, seen, relations): # seen = list of vertices we have been to
    seen.append(start) # Add the start vertex to the list of seen vertices
    if end in relations[start]: # If start is touching end currently, a path exists
        return True
    for vertex in relations[start]: # Otherwise, choose a vertex that is touching the start vertex
        if vertex not in seen: # Make sure we have no been to that vertex
            if checkPath(vertex, end, seen, relations): # For each vertex touching start, go down that path
                return True
    return False # If we never get to the end point, the graph is not connected