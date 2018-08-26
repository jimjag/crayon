﻿using System.Collections.Generic;

namespace Parser
{
    internal enum VariableIdAllocPhase
    {
        REGISTER = 0x1,

        ALLOC = 0x2,

        REGISTER_AND_ALLOC = 0x3,
    }

    public class VariableId
    {
        public VariableId(string name)
        {
            this.Name = name;
            this.UsedByClosure = false;
        }

        public int ID { get; set; }
        public string Name { get; private set; }
        public bool UsedByClosure { get; set; }
        public int ClosureID { get; set; }
    }

    internal class VariableScope
    {
        private VariableScope parentScope = null;
        private VariableScope rootScope = null;
        private VariableScope closureScope = null;
        private VariableScope closureRootScope = null;
        private int closureIdAlloc = 1;

        // A lookup of all ID's that have been registered in this scope up until now.
        private readonly Dictionary<string, VariableId> idsByVar = new Dictionary<string, VariableId>();

        // The following fields are only used in the root scope.
        // flattenedIds is a lookup of all ID's of children branches. Unlike idsByVar, this cannot be used
        // to see if a variable is declared as it is possible that a variable is declared in another branch.
        private Dictionary<string, VariableId> flattenedIds = null;
        // The order that variables are encountered.
        private List<string> rootScopeOrder;

        public int Size { get { return this.flattenedIds.Count; } }

        private VariableScope() { }

        public static VariableScope NewEmptyScope()
        {
            VariableScope scope = new VariableScope()
            {
                rootScopeOrder = new List<string>(),
                flattenedIds = new Dictionary<string, VariableId>(),
            };
            scope.rootScope = scope;
            scope.closureRootScope = scope;
            return scope;
        }

        public static VariableScope CreatedNestedBlockScope(VariableScope parent)
        {
            return new VariableScope()
            {
                parentScope = parent,
                rootScope = parent.rootScope,
                closureRootScope = parent.closureRootScope,
            };
        }

        public static VariableScope CreateClosure(VariableScope parent)
        {
            VariableScope scope = NewEmptyScope();
            scope.closureScope = parent;
            scope.closureRootScope = parent.closureRootScope;
            return scope;
        }

        public void FinalizeScopeIds()
        {
            int id = 0;
            foreach (string varName in this.rootScopeOrder)
            {
                VariableId varId = this.idsByVar[varName];
                if (!varId.UsedByClosure)
                {
                    varId.ID = id++;
                }
            }
        }

        private void MarkVarAsClosureVarThroughParentChain(VariableScope fromScope, VariableScope toScope, VariableId varId)
        {
            if (!varId.UsedByClosure)
            {
                varId.ClosureID = fromScope.closureRootScope.closureIdAlloc++;
                varId.UsedByClosure = true;
            }

            do
            {
                fromScope.idsByVar[varId.Name] = varId;
                fromScope = fromScope.closureScope;
            } while (fromScope != toScope);
        }

        public VariableId RegisterVariable(string value)
        {
            // Before anything else, check to see if this is coming from the closure.
            VariableScope closureWalker = this.closureScope;
            while (closureWalker != null)
            {
                VariableId closureVarId;
                if (closureWalker.idsByVar.TryGetValue(value, out closureVarId))
                {
                    MarkVarAsClosureVarThroughParentChain(this, closureWalker, closureVarId);
                    return closureVarId;
                }
                closureWalker = closureWalker.closureScope;
            }

            VariableId varId;

            // Check if variable is already declared in this or a parent scope already.
            if (rootScope.flattenedIds.ContainsKey(value))
            {
                // The above if statement is a quick check to see if variable used before, anywhere,
                // even if in a parallel branch. This will prevent many unnecessary walks up the parent chain.

                // Variable is already known by this scope. Nothing to do.
                if (!this.idsByVar.TryGetValue(value, out varId))
                {
                    // Check to see if this variable was used by this or any parent scope.
                    VariableScope walker = this.parentScope;
                    while (walker != null)
                    {
                        if (walker.idsByVar.TryGetValue(value, out varId))
                        {
                            // cache this value in the current scope to make the lookup faster in the future
                            this.idsByVar[value] = varId;
                            return varId;
                        }
                        walker = walker.parentScope;
                    }

                    // If you got to this point, that means the variable was used somewhere, but not in the direct
                    // scope parent chain. Grab the same VariableId instance and copy it to this scope.
                    varId = this.rootScope.flattenedIds[value];
                    this.idsByVar[value] = varId;
                }
            }
            else
            {
                // Variable has never been used anywhere. Create a new one and put it in the root bookkeeping.
                varId = new VariableId(value);
                this.idsByVar[value] = varId;
                this.rootScope.flattenedIds[value] = varId;
                this.rootScope.rootScopeOrder.Add(value);
                return varId;
            }
            return varId;
        }

        public VariableId GetVarId(Token variableToken)
        {
            VariableScope walker;
            VariableId varId;
            string name = variableToken.Value;

            // Most common case. Nothing to do if you find it here.
            if (this.idsByVar.TryGetValue(name, out varId))
            {
                return varId;
            }

            // Check closures
            walker = this.closureScope;
            while (walker != null)
            {
                // Note that by the time the lambda var ID allocation begins, the containing scope
                // has already been finished and flattened, so there's no concept of parent scopes here
                // aside from the closure chain.
                if (walker.idsByVar.TryGetValue(name, out varId))
                {
                    MarkVarAsClosureVarThroughParentChain(this, walker, varId);
                    return varId;
                }
                walker = walker.closureScope;
            }

            // Check parent chain
            walker = this;
            while (walker != null)
            {
                if (walker.idsByVar.TryGetValue(name, out varId))
                {
                    return varId;
                }
                walker = walker.parentScope;
            }

            return null;
        }

        public void MergeToParent()
        {
            foreach (VariableId v in this.idsByVar.Values)
            {
                this.parentScope.idsByVar[v.Name] = v;
            }
        }
    }
}
