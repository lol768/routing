﻿//// Itinero - Routing for .NET
//// Copyright (C) 2016 Abelshausen Ben
//// 
//// This file is part of Itinero.
//// 
//// Itinero is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 2 of the License, or
//// (at your option) any later version.
//// 
//// Itinero is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//// GNU General Public License for more details.
//// 
//// You should have received a copy of the GNU General Public License
//// along with Itinero. If not, see <http://www.gnu.org/licenses/>.

//using Itinero.Data.Contracted.Edges;
//using Itinero.Graphs.Directed;
//using System;
//using System.Collections.Generic;

//namespace Itinero.Algorithms.Contracted.EdgeBased
//{
//    /// <summary>
//    /// Directed graph extensions assuming it contains contracted data.
//    /// </summary>
//    public static class DirectedMetaGraphExtensions
//    {
//        /// <summary>
//        /// Add or update edge.
//        /// </summary>
//        /// <returns></returns>
//        public static void AddOrUpdateEdge(this DirectedMetaGraph graph, uint vertex1, uint vertex2, float weight, 
//            bool? direction, uint contractedId)
//        {
//            var current = ContractedEdgeDataSerializer.Serialize(weight, direction);
//            var hasExistingEdge = false;
//            var hasExistingEdgeOnlySameDirection = true;
//            if(graph.UpdateEdge(vertex1, vertex2, (data) => 
//                {
//                    hasExistingEdge = true;
//                    if(ContractedEdgeDataSerializer.HasDirection(data[0], direction))
//                    { // has the same direction.
//                        if (weight < ContractedEdgeDataSerializer.DeserializeWeight(data[0]))
//                        { // the weight is better, just update.
//                            return true;
//                        }
//                        return false;
//                    }
//                    hasExistingEdgeOnlySameDirection = false;
//                    return false;
//                }, new uint[] { current }, contractedId) != Constants.NO_EDGE)
//            { // updating the edge succeeded.
//                return;
//            }
//            if (!hasExistingEdge)
//            { // no edge exists yet.
//                graph.AddEdge(vertex1, vertex2, current, contractedId);
//                return;
//            }
//            else if (hasExistingEdgeOnlySameDirection)
//            { // there is an edge already but it has a better weight.
//                return;
//            }
//            else
//            { // see what's there and update if needed.
//                var forward = false;
//                var forwardWeight = float.MaxValue;
//                var forwardContractedId = uint.MaxValue;
//                var backward = false;
//                var backwardWeight = float.MaxValue;
//                var backwardContractedId = uint.MaxValue;

//                if(direction == null || direction.Value)
//                {
//                    forward = true;
//                    forwardWeight = weight;
//                    forwardContractedId = contractedId;
//                }
//                if(direction == null || !direction.Value)
//                {
//                    backward = true;
//                    backwardWeight = weight;
//                    backwardContractedId = contractedId;
//                }

//                var edgeEnumerator = graph.GetEdgeEnumerator(vertex1);
//                while(edgeEnumerator.MoveNext())
//                {
//                    if(edgeEnumerator.Neighbour == vertex2)
//                    {
//                        float localWeight;
//                        bool? localDirection;
//                        uint localContractedId;
//                        ContractedEdgeDataSerializer.Deserialize(edgeEnumerator.Data0, edgeEnumerator.MetaData0,
//                            out localWeight, out localDirection, out localContractedId);
//                        if(localDirection == null || localDirection.Value)
//                        {
//                            if(localWeight < forwardWeight)
//                            {
//                                forwardWeight = localWeight;
//                                forward = true;
//                                forwardContractedId = localContractedId;
//                            }
//                        }
//                        if (localDirection == null || !localDirection.Value)
//                        {
//                            if (localWeight < backwardWeight)
//                            {
//                                backwardWeight = localWeight;
//                                backward = true;
//                                backwardContractedId = localContractedId;
//                            }
//                        }
//                    }
//                }

//                graph.RemoveEdge(vertex1, vertex2);

//                if(forward && backward &&
//                    forwardWeight == backwardWeight &&
//                    forwardContractedId == backwardContractedId)
//                { // add one bidirectional edge.
//                    graph.AddEdge(vertex1, vertex2,
//                        ContractedEdgeDataSerializer.Serialize(forwardWeight, null), forwardContractedId);
//                }
//                else
//                { // add two unidirectional edges if needed.
//                    if(forward)
//                    { // there is a forward edge.
//                        graph.AddEdge(vertex1, vertex2,
//                            ContractedEdgeDataSerializer.Serialize(forwardWeight, true), forwardContractedId);
//                    }
//                    if (backward)
//                    { // there is a backward edge.
//                        graph.AddEdge(vertex1, vertex2,
//                            ContractedEdgeDataSerializer.Serialize(backwardWeight, false), backwardContractedId);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Tries adding or updating an edge and returns #added and #removed edges.
//        /// </summary>
//        /// <returns></returns>
//        public static void TryAddOrUpdateEdge(this DirectedMetaGraph graph, uint vertex1, uint vertex2, float weight, bool? direction, uint contractedId,
//            out int added, out int removed)
//        {
//            var hasExistingEdge = false;
//            var hasExistingEdgeOnlySameDirection = true;
//            var edgeCount = 0;
//            var edgeEnumerator = graph.GetEdgeEnumerator(vertex1);
//            while(edgeEnumerator.MoveNext())
//            {
//                if (edgeEnumerator.Neighbour == vertex2)
//                {
//                    edgeCount++;
//                    hasExistingEdge = true;
//                    if (ContractedEdgeDataSerializer.HasDirection(edgeEnumerator.Data0, direction))
//                    { // has the same direction.
//                        if (weight < ContractedEdgeDataSerializer.DeserializeWeight(edgeEnumerator.Data0))
//                        { // the weight is better, just update.
//                            added = 1;
//                            removed = 1;
//                            return;
//                        }
//                        hasExistingEdgeOnlySameDirection = false;
//                    }
//                }
//            }

//            if (!hasExistingEdge)
//            { // no edge exists yet.
//                added = 1;
//                removed = 0;
//                return;
//            }
//            else if (hasExistingEdgeOnlySameDirection)
//            { // there is an edge already but it has a better weight.
//                added = 0;
//                removed = 0;
//                return;
//            }
//            else
//            { // see what's there and update if needed.
//                var forward = false;
//                var forwardWeight = float.MaxValue;
//                var forwardContractedId = uint.MaxValue;
//                var backward = false;
//                var backwardWeight = float.MaxValue;
//                var backwardContractedId = uint.MaxValue;

//                if (direction == null || direction.Value)
//                {
//                    forward = true;
//                    forwardWeight = weight;
//                    forwardContractedId = contractedId;
//                }
//                if (direction == null || !direction.Value)
//                {
//                    backward = true;
//                    backwardWeight = weight;
//                    backwardContractedId = contractedId;
//                }

//                edgeEnumerator = graph.GetEdgeEnumerator(vertex1);
//                while (edgeEnumerator.MoveNext())
//                {
//                    if (edgeEnumerator.Neighbour == vertex2)
//                    {
//                        float localWeight;
//                        bool? localDirection;
//                        uint localContractedId;
//                        ContractedEdgeDataSerializer.Deserialize(edgeEnumerator.Data0, edgeEnumerator.MetaData0,
//                            out localWeight, out localDirection, out localContractedId);
//                        if (localDirection == null || localDirection.Value)
//                        {
//                            if (localWeight < forwardWeight)
//                            {
//                                forwardWeight = localWeight;
//                                forward = true;
//                                forwardContractedId = localContractedId;
//                            }
//                        }
//                        if (localDirection == null || !localDirection.Value)
//                        {
//                            if (localWeight < backwardWeight)
//                            {
//                                backwardWeight = localWeight;
//                                backward = true;
//                                backwardContractedId = localContractedId;
//                            }
//                        }
//                    }
//                }

//                removed = edgeCount;
//                added = 0;

//                if (forward && backward &&
//                    forwardWeight == backwardWeight &&
//                    forwardContractedId == backwardContractedId)
//                { // add one bidirectional edge.
//                    added++;
//                }
//                else
//                { // add two unidirectional edges if needed.
//                    if (forward)
//                    { // there is a forward edge.
//                        added++;
//                    }
//                    if (backward)
//                    { // there is a backward edge.
//                        added++;
//                    }
//                }
//            }
//        }
//    }
//}