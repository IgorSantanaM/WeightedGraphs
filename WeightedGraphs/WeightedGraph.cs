using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WeightedGraphs
{
    public class WeightedGraph
    {
            private class Node
            {
                public string label;
                private List<Edge> edges = new();    

                public Node(string label)
                {
                    this.label = label;
                }
                public override string ToString()
                {
                    return label;
                }

                public void AddEdge(Node to, int weight) => 
                    edges.Add(new Edge(this, to, weight));

                public List<Edge> GetEdges() => edges;
            }

            private class Edge
            {
                public Node from;
                public Node to;
                public int weight;

                public Edge(Node from, Node to, int weight)
                {
                    this.from = from;
                    this.to = to;
                    this.weight = weight;
                }

                public override string ToString()
                {
                    return from + "->" + to;
                }
            }

            private Dictionary<string, Node> nodes = new();

            public void AddNode(string label) =>
                nodes.TryAdd(label, new Node(label));

            public void AddEdge(string from, string to, int weight)
            {
                var fromNode = nodes[from];
                var toNode = nodes[to];

                if (fromNode is null || toNode is null)
                    throw new Exception();

                fromNode.AddEdge(toNode, weight);
                toNode.AddEdge(fromNode, weight);
            }

            public Path GetShortestPath(string from, string to)
            {
                var fromNode = nodes[from];

                if (fromNode is null)
                    throw new NullReferenceException("Null reference");

                var toNode = nodes[to];

                Dictionary<Node, int> distances = new();

                foreach (var node in nodes.Values)
                    distances[node] = int.MaxValue;
                distances[nodes[from]] = 0;

                Dictionary<Node, Node> previousNodes = new();

                HashSet<Node> visited = new();

                PriorityQueue<Node, int> queue = new PriorityQueue<Node, int>();

                queue.Enqueue(fromNode, 0);

                while(queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    visited.Add(current);

                    foreach (var edge in current.GetEdges())
                    {
                        if (visited.Contains(edge.to))
                            continue;

                        var newDistance = distances[current] + edge.weight;

                        if(newDistance < distances[edge.to])
                        {
                            distances[edge.to] = newDistance;
                            previousNodes[edge.to] = current;
                            queue.Enqueue(edge.to, newDistance); 
                        }
                    }
                }
                
                return BuildPath(previousNodes, toNode);
            }

            public void Print()
            {
                foreach (var node in nodes.Values)
                {
                    var edges = node.GetEdges();
                    if (!edges.Any())
                        Console.WriteLine(node + "is connected to " + edges);
                }
            }

            private Path BuildPath(Dictionary<Node, Node> previousNodes, Node toNode)
            {
                Stack<Node> stack = new();
                stack.Push(toNode);

                var previous = previousNodes[toNode];
                while (previous is not null)
                {
                    stack.Push(previous);
                    previous = previousNodes[previous];
                }
                var path = new Path();

                while (stack.Any())
                    path.Add(stack.Pop().label);

                return path;
            }
        }
    }
