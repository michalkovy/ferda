using Ice;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Object = Ice.Object;

namespace Ferda.Modules.Boxes.Language.Lambda
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            iceObject = null;
            functions = null;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return new string[0];
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "Language.Lambda";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            // Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                /*case Common.PropTotalNumberOfRelevantQuestions:
                    return new DoubleTI(Common.TotalNumberOfRelevantQuestions(Func));
                case Common.PropNumberOfVerifications:
                    return new LongTI(Common.NumberOfVerifications(Func));
                case Common.PropNumberOfHypotheses:
                    return new LongTI(Common.NumberOfHypotheses(Func));
                case Common.PropStartTime:
                    return new DateTimeTI(Common.StartTime(Func));
                case Common.PropEndTime:
                    return new DateTimeTI(Common.EndTime(Func));
				case Common.PropTotalTime:
					return new DateTimeTI(Common.EndTime(Func) - Common.StartTime(Func));*/
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
        }
		
		public override SocketInfo[] GetAdditionalSockets(String[] localePrefs, BoxModuleI boxModule)
		{
			int variablesCount = boxModule.GetPropertyInt("VariablesCount");
			SocketInfo[] resultSocketInfo = new SocketInfo[variablesCount*2];
			for(int i = 0; i < variablesCount; i++)
			{
				SocketInfo socketInfoVariable = new SocketInfo(
					String.Format("Variable{0}", i),
					String.Format("Variable {0}", i),
					String.Format("Variable {0}", i),
					null,
					new BoxType[1]{new BoxType(null, new NeededSocket[0])},
					new String[0],
					true);
				BoxType[] variableBoxType;
				BoxModulePrx[] connectedBoxes = boxModule.GetConnections(String.Format("Variable{0}", i));
				if((connectedBoxes != null) && (connectedBoxes.Length > 0))
				{
					StringCollection functionIceIds = new StringCollection();
					StringCollection oldFunctionIceIds;
					Dictionary<string, StringCollection> neededSockets = new Dictionary<string, StringCollection>();
					functionIceIds.AddRange(connectedBoxes[0].getFunctionsIceIds());
					foreach (BoxModulePrx connectedBox in connectedBoxes)
					{
						oldFunctionIceIds = functionIceIds;
						functionIceIds = new StringCollection();
						foreach (string functionIceId in connectedBox.getFunctionsIceIds())
						{
							if(oldFunctionIceIds.Contains(functionIceId))
							{
								functionIceIds.Add(functionIceId);
							}
						}
						
						List<SocketInfo> sockets = new List<SocketInfo>(connectedBox.getMyFactory().getSockets());
						sockets.AddRange(connectedBox.getAdditionalSockets());
						foreach(SocketInfo socket in sockets)
						{
							BoxModulePrx[] connections = connectedBox.getConnections(socket.name);
							if((connections != null) && (connections.Length > 0))
							{
								string socketIceId = null;
								StringCollection socketFunctionIceIds = new StringCollection();
								StringCollection socketOldFunctionIceIds;
								socketFunctionIceIds.AddRange(connections[0].getFunctionsIceIds());
								foreach (BoxModulePrx connection in connections)
								{
									socketOldFunctionIceIds = socketFunctionIceIds;
									socketFunctionIceIds = new StringCollection();
									foreach(string iceId in connection.getFunctionsIceIds())
									{
										if(socketOldFunctionIceIds.Contains(iceId))
										{
											socketFunctionIceIds.Add(iceId);
										}
									}
								}
								StringCollection boxTypeSocketIceIds = new StringCollection();
								foreach(BoxType boxType in socket.socketType)
								{
									if(socketFunctionIceIds.Contains(boxType.functionIceId))
									{
										socketIceId = boxType.functionIceId;
										if(!boxTypeSocketIceIds.Contains(boxType.functionIceId))
											boxTypeSocketIceIds.Add(boxType.functionIceId);
									}
								}
								
								if(neededSockets.ContainsKey(socket.name))
								{
									StringCollection boxTypeSocketIceIdsOld = neededSockets[socket.name];
									foreach(string iceId in boxTypeSocketIceIds)
									{
										if(!boxTypeSocketIceIdsOld.Contains(iceId))
											boxTypeSocketIceIds.Remove(iceId);
									}
								}
								
								neededSockets[socket.name] = boxTypeSocketIceIds;
							}
						}
					}
					List<NeededSocket> neededSocketsOne = new List<NeededSocket>(neededSockets.Keys.Count);
					foreach (KeyValuePair<string, StringCollection> neededSocket in neededSockets)
					{
						neededSocketsOne.Add(new NeededSocket(neededSocket.Key,
															  (neededSocket.Value.Count > 0) ? neededSocket.Value[0] : null));
					}
					variableBoxType = new BoxType[1]{new BoxType((functionIceIds.Count > 0) ? null : functionIceIds[0],
																 neededSocketsOne.ToArray())};
				}
				else
				{
					variableBoxType = new BoxType[1]{new BoxType(null, new NeededSocket[0])};
				}
				SocketInfo socketInfoVariableValue = new SocketInfo(
					String.Format("VariableValue{0}", i),
					String.Format("Value of variable {0}", i),
					String.Format("Value of variable {0}", i),
					null,
					variableBoxType,
					new String[0],
					true);
				resultSocketInfo[2*i] = socketInfoVariable;
				resultSocketInfo[2*i + 1] = socketInfoVariableValue;
			}
			return resultSocketInfo;
		}
		
		/*
		public override PropertyInfo[] GetAdditionalProperties(String[] localePrefs, BoxModuleI boxModule)
		{
			int variablesCount = boxModule.GetPropertyInt("VariablesCount");
			PropertyInfo[] resultPropertyInfo = new PropertyInfo[variablesCount];
			for(int i = 0; i < variablesCount; i++)
			{
				
			}
			return resultPropertyInfo;
		}*/
		/// <summary>
		/// Constructor
		/// </summary>
		/// <summary>
		/// Constructor
		/// </summary>
		/// <summary>
		/// Constructor
		/// </summary>
		/// <summary>
		/// Constructor
		/// </summary>
		/// <summary>
		/// Constructor
		/// </summary>
    }
}
