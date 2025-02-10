using WeightedGraphs;

var graph = new WeightedGraph();
graph.AddNode("A");
graph.AddNode("B");
graph.AddNode("C");
graph.AddEdge("A", "B", 1);
graph.AddEdge("B", "C", 2);
graph.AddEdge("A", "C", 10);

graph.GetShortestPath("A", "C");
