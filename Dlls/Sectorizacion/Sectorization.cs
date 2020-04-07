using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

using NLog;

using SectorizacionDll.Properties;


namespace GeneraSectorizacionDll
{
	public enum PrefixType : byte { LCI, LCE, INT, ATS, PUB, PABX, MW, ADMIN, PPAB, PP=32, FREE = 0xFF }
    public enum SectorizationResult : byte
   {
      Ok, Error, OtherSectReceived, EmptySectorization,
      TwoMaintenanceSectors, MissingRealSectors, MultiNucleoWithoutGroup,
		IOLError, TimeOutError, NotPrincipalIOL, InProgress = 0xFF
   };
	public enum Tipo_Sectorizacion { Sectorizacion_Completa, Sectorizacion_Radio, Sectorizacion_Telefonia };

    /// <summary>
    /// 
    /// </summary>
	public class SectorizationException : Exception
	{
		public SectorizationResult Result;

		public SectorizationException(SectorizationResult result, string msg)
			 : base(msg)
		{
			Result = result;
		}
	}

    /// <summary>
    /// 
    /// </summary>
	internal class RDEndPoint
	{
		public int Position;
		public string Frecuency;
		public string Literal;
		public int Priority;
		public int PrioridadSIP;
		public string EstadoAsignacion;
		public List<string> EstadoAltavoces;
		public Dictionary<string, string> EstadoRecursos;
        public bool SupervisionPortadora;

		public RDEndPoint()
		{
			EstadoRecursos = new Dictionary<string, string>();
			EstadoAltavoces = new List<string>();
		}

		public RDEndPoint(RDEndPoint ep)
		{
			Position = ep.Position;
			Frecuency = ep.Frecuency;
			Literal = ep.Literal;
			Priority = ep.Priority;
			PrioridadSIP = ep.PrioridadSIP;
			EstadoAsignacion = ep.EstadoAsignacion;
			EstadoAltavoces = ep.EstadoAltavoces;
			EstadoRecursos = ep.EstadoRecursos;
            SupervisionPortadora = ep.SupervisionPortadora;
		}

	}

    /// <summary>
    /// 
    /// </summary>
	internal class TLFEndPoint
	{
		public int Position;
		public string Name;
		public PrefixType Prefix;
		public string Literal;
		public string PublicNumber;
		// public int Ring;
		public string Origin;
		public string Group;
		public int Priority;
		public int PriorityR2;
		public string IdDestino;
		public uint TipoDestino;
		public string TipoAcceso;

        //Para los recursos ATS del panel de LC, almacena el identificador del destino LCEN
        //que puede tener asignado
        public string strIdDestinoLCEN;

		public TLFEndPoint()
		{
		}

		public TLFEndPoint(TLFEndPoint ep)
		{
			Position = ep.Position;
			Name = ep.Name;
			Prefix = ep.Prefix;
			Literal = ep.Literal;
			PublicNumber = ep.PublicNumber;
			Origin = ep.Origin;
			Group = ep.Group;
			Priority = ep.Priority;
			PriorityR2 = ep.PriorityR2;
			IdDestino = ep.IdDestino;
			TipoDestino = ep.TipoDestino;
			TipoAcceso = ep.TipoAcceso;
            strIdDestinoLCEN = ep.strIdDestinoLCEN;
		}
   }

    /// <summary>
    /// 
    /// </summary>
	internal class SectorGroupInfo
	{
		public string Name;
		public bool Especial;
		public List<int> Sectors = new List<int>();
		public TLFEndPoint[] TlfEndPointsE;
		public TLFEndPoint[] TlfEndPointsP;
	}

    /// <summary>
    /// 
    /// </summary>
    public struct PermisoRed
    {
        public bool Llamar;
        public bool Recibir;
    }

    /// <summary>
    /// 
    /// </summary>
    internal class UserInfo 
	{
		struct Indexer
		{
			public int Begin;
			public int End;
			public int Last;

			public Indexer(int begin, int end)
			{
				Begin = Last = begin;
				End = end;
			}
		}
		public struct StAbonado
		{
			public uint Prefijo;
			public string Numero;
		}

		public string IdSistema;
		public string IdNucleo;
		public string IdSectorizacion;

		public SectorInfo Sector;
		public string Top;
		public string IdUsuario;

		public string Name;
		public string Literal;
		public int R2Priority = Int32.MaxValue;
		//public int PttPriority;
		//public int TxPriority;

		public uint NumLLamadasEntrantesIDA;
		public uint NumLlamadasEnIDA;
		public uint NumFreqPagina;
		public uint NumPagFreq;
		public uint NumDestinosInternosPag;
		public uint NumPagDestinosInt;
		public bool Intrusion, Intruido;
		public uint KAP, KAM;
        public uint NumEnlacesAI;
        public bool GrabacionEd137;
        public bool TransConConsultaPrev, TransDirecta, Conferencia, Escucha,
            Retener, Captura, Redireccion,
            RepeticionUltLlamada, RellamadaAut, TeclaPrioridad,
            Tecla55mas1, Monitoring, CoordinadorTF,
            CoordinadorRD, IntegracionRDTF, LlamadaSelectiva,
            GrupoBSS, LTT, SayAgain, InhabilitacionRedirec, Glp;

		public List<StAbonado> PublicNumbers = new List<StAbonado>();
		public List<int> ATSNumbers = new List<int>();
//		public List<string> MergedUsers = new List<string>();
		public TLFEndPoint[] TlfEndPoints;// = new TLFEndPoint[67];
		public TLFEndPoint[] LCEndPoints;// = new TLFEndPoint[67];
		public TLFEndPoint[] AgEndPoints;// = new TLFEndPoint[67];
		public RDEndPoint[] RdEndPoints;// = new RDEndPoint[100];

        public Dictionary<string, PermisoRed> PermisoRedes = new Dictionary<string, PermisoRed>();

        Dictionary<string, int> _LceGroupsPriority = new Dictionary<string, int>();
        //Dictionary<string, int> _LceGroupsResourcesCount = new Dictionary<string, int>();
		Dictionary<string, string> _InternalDstInOPE = new Dictionary<string, string>();
		Dictionary<string, TLFEndPoint> _TlfEndPointsByName = new Dictionary<string, TLFEndPoint>();
		Dictionary<string, TLFEndPoint> _LcEndPointsByName = new Dictionary<string, TLFEndPoint>();
		Dictionary<string, TLFEndPoint> _AgEndPointsByName = new Dictionary<string, TLFEndPoint>();
		Dictionary<string, RDEndPoint>[] _RdEndPointsByName;// = new Dictionary<string, RDEndPoint>();
		Indexer[] _TlfIndex; /* = new Indexer[] { new Indexer(1, int.MaxValue), new Indexer(1, int.MaxValue), new Indexer(1, int.MaxValue) };*/
		Indexer[] _RdIndex; /* = new Indexer[]  {	new Indexer(1, 10), new Indexer(11, 20),
													new Indexer(21, 30), new Indexer(31, 40),
													new Indexer(41, 50), new Indexer(51, 60),
													new Indexer(61, 70), new Indexer(71, 80),
													new Indexer(81, 90), new Indexer(91, 100)};*/

        //Contendrá la lista de Destinos ATS configurados en el panel de línea Caliente
        //Que pueden tener asignado algún destino LCEN
        System.Collections.Hashtable _LcEndPointsATSWithLCEN = null;

		public void CreaUser(UserInfo usr)
		{
			NumDestinosInternosPag = usr.NumDestinosInternosPag;
			NumFreqPagina = usr.NumFreqPagina;
			NumPagDestinosInt = usr.NumPagDestinosInt;
			NumPagFreq = usr.NumPagFreq;
			RdEndPoints = new RDEndPoint[NumFreqPagina * NumPagFreq];
			TlfEndPoints = new TLFEndPoint[NumDestinosInternosPag * NumPagDestinosInt];
			LCEndPoints = new TLFEndPoint[NumDestinosInternosPag * NumPagDestinosInt];
			AgEndPoints = new TLFEndPoint[NumDestinosInternosPag * NumPagDestinosInt];

            _TlfIndex = new Indexer[] { new Indexer(1, (int)(NumDestinosInternosPag * NumPagDestinosInt) + 1), 
                                        new Indexer(1, 19), 
                                        new Indexer(1, int.MaxValue) };

			_RdIndex = new Indexer[NumPagFreq];
			for (int i = 0; i < NumPagFreq; i++)
			{
				_RdIndex[i] = new Indexer((int)1 + (int)(i * NumFreqPagina), (int)(i + 1) * (int)NumFreqPagina);
			}

			_RdEndPointsByName = new Dictionary<string, RDEndPoint>[NumPagFreq];
			for (int i = 0; i < NumPagFreq; i++)
			{
				_RdEndPointsByName[i] = new Dictionary<string, RDEndPoint>();
			}

            //Se inicializa el atributo intruido con el valor configurado en el sector
            Intruido = usr.Intruido;
		}
		
		public UserInfo(SectorInfo sector, string top, string num)
		{
			Sector = sector;
			Top = top;
			IdUsuario = num;
		}

		public UserInfo(UserInfo user)
		{
			Top = user.Top;
			IdUsuario = user.IdUsuario;
			Name = user.Name;
			//TxPriority = user.TxPriority;
		}

		public void MergeUser(UserInfo user)
		{
			R2Priority = Math.Min(R2Priority, user.R2Priority);
			//PttPriority = Math.Max(PttPriority, user.PttPriority);
			ATSNumbers.AddRange(user.ATSNumbers);
			PublicNumbers.AddRange(user.PublicNumbers);
//			MergedUsers.Add(user.IdUsuario/* OJO. Esto antes era un int */);

			IdNucleo = user.IdNucleo;
			IdSistema = user.IdSistema;
			IdSectorizacion = user.IdSectorizacion;
			Name = user.Name;


			// ParametrosSector

			NumLLamadasEntrantesIDA = NumLLamadasEntrantesIDA > user.NumLLamadasEntrantesIDA ? NumLLamadasEntrantesIDA : user.NumLLamadasEntrantesIDA;
			NumLlamadasEnIDA = NumLlamadasEnIDA > user.NumLlamadasEnIDA ? NumLlamadasEnIDA : user.NumLlamadasEnIDA;
			NumFreqPagina = NumFreqPagina > user.NumFreqPagina ? NumFreqPagina : user.NumFreqPagina;
			NumPagFreq = NumPagFreq > user.NumPagFreq ? NumPagFreq : user.NumPagFreq;
			NumDestinosInternosPag = NumDestinosInternosPag > user.NumDestinosInternosPag ? NumDestinosInternosPag : user.NumDestinosInternosPag;
			NumPagDestinosInt = NumPagDestinosInt > user.NumPagDestinosInt ? NumPagDestinosInt : user.NumPagDestinosInt;
			Intrusion |= user.Intrusion;
            // Si algun sector no tiene permiso intruido, la agrupación tampoco la tiene
            //Solo lo tendrán si todos tienen permiso de intruido
			Intruido &= user.Intruido;
			KAP = KAP > user.KAP ? KAP : user.KAP;
			KAM = KAM > user.KAM ? KAM : user.KAM;
            NumEnlacesAI = NumEnlacesAI > user.NumEnlacesAI ? NumEnlacesAI : user.NumEnlacesAI;
            GrabacionEd137 |= user.GrabacionEd137;

            // Teclas sector
            TransConConsultaPrev |= user.TransConConsultaPrev;
            TransDirecta |= user.TransDirecta;
            Conferencia |= user.Conferencia;
            Escucha |= user.Escucha;
            Retener |= user.Retener;
            Captura|= user.Captura;
            Redireccion|= user.Redireccion;
            RepeticionUltLlamada|= user.RepeticionUltLlamada;
            RellamadaAut|= user.RellamadaAut;
            TeclaPrioridad|= user.TeclaPrioridad;
            Tecla55mas1|= user.Tecla55mas1;
            Monitoring|= user.Monitoring;
            CoordinadorTF|= user.CoordinadorTF;
            CoordinadorRD|= user.CoordinadorRD;
            IntegracionRDTF|= user.IntegracionRDTF;
            LlamadaSelectiva|= user.LlamadaSelectiva;
            GrupoBSS|= user.GrupoBSS;
            LTT|= user.LTT;
            SayAgain|= user.SayAgain;
            InhabilitacionRedirec |= user.InhabilitacionRedirec;
            Glp |= user.Glp;

            // permisos redes
            foreach (string red in user.PermisoRedes.Keys)
            {
                if (!PermisoRedes.ContainsKey(red))
                {
                    PermisoRedes.Add(red, user.PermisoRedes[red]);
                }
                else
                {
                    PermisoRed pr;
                    PermisoRedes.TryGetValue(red, out pr);
                    pr.Llamar |= user.PermisoRedes[red].Llamar;
                    pr.Recibir |= user.PermisoRedes[red].Recibir;
                    PermisoRedes[red] = pr;
                }
            }

            if (user.RdEndPoints != null && user.RdEndPoints.Length > RdEndPoints.Length)
				Array.Resize(ref RdEndPoints, user.RdEndPoints.Length);

			if (user.TlfEndPoints != null && user.TlfEndPoints.Length > TlfEndPoints.Length)
				Array.Resize(ref TlfEndPoints, user.TlfEndPoints.Length);
			if (user.LCEndPoints != null && user.LCEndPoints.Length > LCEndPoints.Length)
				Array.Resize(ref LCEndPoints, user.LCEndPoints.Length);
			if (user.AgEndPoints != null && user.AgEndPoints.Length > AgEndPoints.Length)
				Array.Resize(ref AgEndPoints, user.AgEndPoints.Length);

			if (user.LCEndPoints != null)
			{
                foreach (TLFEndPoint ep in user.LCEndPoints)
				{
					if ((ep != null) && (ep.Prefix == PrefixType.LCE) && (ep.Group != null))
					{
						int lceGroupPriority;

						if (!_LceGroupsPriority.TryGetValue(ep.Group, out lceGroupPriority))
						{
							_LceGroupsPriority[ep.Group] = ep.Priority;
						}
						else
						{
							_LceGroupsPriority[ep.Group] = Math.Min(lceGroupPriority, ep.Priority);
                        }
					}
				}
			}
		}

		public void MergeColateralsTf(UserInfo user, Sectorization sct)
		{
            

			if (user.TlfEndPoints == null)
				return;

            int iMaxNumTeclasPanelTlf = TlfEndPoints.Length;

			foreach (TLFEndPoint userEP in user.TlfEndPoints)
			{
				if (userEP == null)
				{
					continue;
				}

				string epId = userEP.Name + userEP.Prefix;
				TLFEndPoint ep = null;

				if (!_TlfEndPointsByName.TryGetValue(epId, out ep))
				{
					UserInfo dstUser = null;
					ep = new TLFEndPoint(userEP);
					ep.PriorityR2 = userEP.PriorityR2;

					if (ep.Prefix == PrefixType.ATS)
					{
					    ep.Origin = user.ATSNumbers[0].ToString();
					//    ep.PriorityR2 = userEP.PriorityR2;

					//    //Debug.Assert(ep.Origin >= 0);
					}
					else if (ep.Prefix == PrefixType.INT)
					{
						if (!sct.ListOfUsersByName.TryGetValue(ep.Name, out dstUser) || (dstUser.Top==null) || (dstUser.Top == Top) ||
							_InternalDstInOPE.ContainsKey(dstUser.Top.ToString() + ep.Prefix)) // || !sct.ListOfDominantUsers.Contains(dstUser.Sector.IdSector))
						{
							continue;
						}

						// Cambiar el IdDestino de la posición porque puede ser que se trate 
						// de un usuario que pertenece a una agrupación
						ep.Literal = dstUser.Sector.Ucs.UserE.Literal;	// dstUser.Sector.Name; //ep.IdDestino = 
					}
                    else if ((ep.Prefix == PrefixType.LCE) && (ep.Group != null))
                    {
                        ep.Priority = _LceGroupsPriority[ep.Group];
                        ep.Origin = ep.Group;

                        Indexer idx = _TlfIndex[1];
                        int pos = Array.FindIndex(LCEndPoints, (int)idx.Begin, (int)(idx.End - idx.Begin),
                        delegate(TLFEndPoint obj) { return ((obj != null) && (obj.Group == ep.Group)); });

                        if (pos >= 0)
                        {
                            for (ep.Position = idx.End; ep.Position < LCEndPoints.Length && LCEndPoints[ep.Position] != null; ep.Position++) ;
                            if (ep.Position >= LCEndPoints.Length) Array.Resize(ref LCEndPoints, (int)(ep.Position + 1));
                        }
                    }

					ep.Position = GetTlfPosition(ep);
                    //if (ep.Position > 0 && ep.Position <NumDestinosInternosPag * NumPagDestinosInt)
                    if (ep.Position > 0 && ep.Position < iMaxNumTeclasPanelTlf)
					{
						if (dstUser != null)
						{
							_InternalDstInOPE[dstUser.Top.ToString() + ep.Prefix] = null;
						}

						TLFEndPoint replaceEp = TlfEndPoints[ep.Position];
						if ((replaceEp != null) && (replaceEp.Prefix == PrefixType.LCE) && (replaceEp.Group != null))
						{
							for (int i = _TlfIndex[1].End; i < TlfEndPoints.Length; i++)
							{
								if (TlfEndPoints[i].Group == replaceEp.Group)
								{
									TlfEndPoints[i] = null;
								}
							}
						}

						TlfEndPoints[ep.Position] = ep;
						_TlfEndPointsByName[epId] = ep;
					}
				}
				else
				{
					// La tecla ATS ya está en el usuario. El origen y prioridad de la misma se recalculan
					ep.Priority = Math.Min(ep.Priority, userEP.Priority);
					ep.PriorityR2 = Math.Min(ep.PriorityR2, userEP.PriorityR2);

					if (ep.Prefix == PrefixType.ATS)
					{
                        ep.Origin = ATSNumbers[ATSNumbers.Count - 1].ToString();  // ATSNumbers está ordenada de mayor a menor -> Se coge el menor
                        //if ((user.ATSNumbers.Count > 0) && (user.R2Priority < ep.PriorityR2) ||
                        //    ((user.R2Priority == ep.PriorityR2) && (user.ATSNumbers[0] < ATSNumbers[0])))
                        //{
                        //    ep.Origin = user.ATSNumbers[0].ToString(); //user.IdUsuario; // ATSNumbers.IndexOf(user.ATSNumbers[0]);
                        //    //Debug.Assert(ep.Origin >= 0);
                        //}

						//ep.PriorityR2 = Math.Min(ep.PriorityR2, user.R2Priority);
					}
				}
			}
		}

		public void MergeColateralsLc(UserInfo user, Sectorization sct)
		{
			if (user.LCEndPoints == null)
				return;

			foreach (TLFEndPoint userEP in user.LCEndPoints)
			{
				if (userEP == null)
				{
					continue;
				}

				string epId = userEP.Name + userEP.Prefix;
				TLFEndPoint ep = null;

				if (!_LcEndPointsByName.TryGetValue(epId, out ep))
				{
					UserInfo dstUser = null;
					ep = new TLFEndPoint(userEP);
                    ep.Position = GetTlfPosition(ep);

                    if (ep.Position > NumEnlacesAI)
                        continue;

					if (ep.Prefix == PrefixType.LCI)
					{
						if (!sct.ListOfUsersByName.TryGetValue(ep.Name, out dstUser) || (dstUser.Top == null) || (dstUser.Top == Top) ||
							_InternalDstInOPE.ContainsKey(dstUser.Top.ToString() + ep.Prefix))
						{
							continue;
						}

						// Cambiar el IdDestino de la posición porque puede ser que se trate 
						// de un usuario que pertenece a una agrupación
						ep.Literal = dstUser.Sector.Ucs.UserE.Literal; //dstUser.Sector.Name; // ep.IdDestino = 
					}
					else if ((ep.Prefix == PrefixType.LCE) && (ep.Group != null))
					{
                        ep.Priority = _LceGroupsPriority[ep.Group];

						Indexer idx = _TlfIndex[1];

                        int pos = Array.FindIndex(LCEndPoints, (int)idx.Begin, (int)(idx.End - idx.Begin),
                        delegate(TLFEndPoint obj) { return ((obj != null) && (obj.Group == ep.Group)); });

                        if (pos >= 0)
                        {
                            if (Array.FindAll(LCEndPoints, delegate(TLFEndPoint obj) { return ((obj != null) && (obj.Group == ep.Group)); }).Length ==
                            sct.LceGroupsResourcesCount[ep.Group] - 1)
                            {
                                int originalPos = pos;

                                while (pos > 0)
                                {
                                    pos = Array.FindIndex(LCEndPoints, pos, (int)(idx.End - idx.Begin),
                                                delegate(TLFEndPoint obj) { return ((obj != null) && (obj.Group == ep.Group)); });
                                    if (pos > 0)
                                        LCEndPoints[pos] = null;
                                }

                                ep.Literal = ep.Group;
                                ep.Position = originalPos;
                                LCEndPoints[ep.Position] = ep;
                                _LcEndPointsByName[epId] = ep;

                                continue;
                                //for (ep.Position = idx.End; ep.Position < LCEndPoints.Length && LCEndPoints[ep.Position] != null; ep.Position++) ;
                                //if (ep.Position >= LCEndPoints.Length) Array.Resize(ref LCEndPoints, (int)(ep.Position + 1));
                            }
                            else
                            {
                                LCEndPoints[ep.Position] = ep;
                                _LcEndPointsByName[epId] = ep;

                                continue;
                            }
                        }
					}
                    else if (ep.Prefix == PrefixType.ATS)
                    {
                        if (null == _LcEndPointsATSWithLCEN)
                        {
                            _LcEndPointsATSWithLCEN = new System.Collections.Hashtable();
                        }

                        //Se limpia el grupo
                        ep.Group = string.Empty;

                        if (!string.IsNullOrWhiteSpace(ep.strIdDestinoLCEN))
                        {
                            //Creamos el destino LCEN asociado al Destino ATS
                            //para añadirlo a la lista.
                            TLFEndPoint epLCEN = new TLFEndPoint(ep);

                            epLCEN.Prefix = PrefixType.LCE;
                            epLCEN.Name = ep.strIdDestinoLCEN;
                            epLCEN.IdDestino = ep.strIdDestinoLCEN;
                            //El literal del destino LCEN será  su identificador
                            epLCEN.Literal = ep.strIdDestinoLCEN;
                            epLCEN.Group = string.Empty;
                            epLCEN.strIdDestinoLCEN = string.Empty;

                            //Se añade el destino de línea caliente asociado al Destino ATS
                            if (!_LcEndPointsATSWithLCEN.ContainsKey(epId))
                            {
                                List<TLFEndPoint> objList = new List<TLFEndPoint>();

                                objList.Add(epLCEN);
                                _LcEndPointsATSWithLCEN.Add(epId, objList);
                            }
                            else
                            {
                                //Si ya existe el destino ATS, se añade el destino LCEN a la lista
                                if (null != _LcEndPointsATSWithLCEN[epId])
                                {
                                    ((List<TLFEndPoint>)_LcEndPointsATSWithLCEN[epId]).Add(epLCEN);
                                }
                            }
                        }

                        //Se actualiza el origen R2
                        ep.Origin = user.ATSNumbers[0].ToString();
                    }

                    //ep.Position = GetTlfPosition(ep);
					if (ep.Position > 0)
					{
						if (dstUser != null)
						{
							_InternalDstInOPE[dstUser.Top.ToString() + ep.Prefix] = null;
						}

						TLFEndPoint replaceEp = LCEndPoints[ep.Position];
						if ((replaceEp != null) && (replaceEp.Prefix == PrefixType.LCE) && (replaceEp.Group != null))
						{
							for (int i = _TlfIndex[1].End; i < LCEndPoints.Length; i++)
							{
								if (LCEndPoints[i].Group == replaceEp.Group)
								{
									LCEndPoints[i] = null;
								}
							}
						}

						LCEndPoints[ep.Position] = ep;
						_LcEndPointsByName[epId] = ep;
					}
				}
				else
				{
                    //Si existe el destino ATS
					ep.Priority = Math.Min(ep.Priority, userEP.Priority);

                    if (ep.Prefix == PrefixType.ATS)
                    {
                        //Se comprueba si tiene asociado un destino ATS
                        if (!string.IsNullOrWhiteSpace(userEP.strIdDestinoLCEN))
                        {
                            TLFEndPoint epLCEN = new TLFEndPoint(ep);

                            epLCEN.Prefix=PrefixType.LCE;
                            epLCEN.Name=userEP.strIdDestinoLCEN;
                            epLCEN.IdDestino=userEP.strIdDestinoLCEN;
                            //El literal del destino LCEN será  su identificador
                            epLCEN.Literal=userEP.strIdDestinoLCEN;
                            epLCEN.Origin=userEP.Origin;

                            epLCEN.Group = string.Empty;
                            epLCEN.strIdDestinoLCEN = string.Empty;

                            //Se añade el destino de línea caliente asociado al Destino ATS
                            if (!_LcEndPointsATSWithLCEN.ContainsKey(epId))
                            {
                                List<TLFEndPoint> objList = new List<TLFEndPoint>();

                                objList.Add(epLCEN);
                                _LcEndPointsATSWithLCEN.Add(epId, objList);
                            }
                            else
                            {
                                //Si ya existe el destino ATS, se añade el destino LCEN
                                if (null != _LcEndPointsATSWithLCEN[epId])
                                {
                                    ((List<TLFEndPoint>)_LcEndPointsATSWithLCEN[epId]).Add(epLCEN);
                                }
                            }
                        }

                        ep.Origin = ATSNumbers[ATSNumbers.Count - 1].ToString();  // ATSNumbers está ordenada de mayor a menor -> Se coge el menor
                    }

					//if (ep.Prefix == PrefixType.ATS)
					//{
					//    if ((user.ATSNumbers.Count > 0) && ((ep.Origin == Int32.MaxValue) || (user.R2Priority < ep.PriorityR2)) ||
					//        ((user.R2Priority == ep.PriorityR2) && (user.ATSNumbers[0] < ATSNumbers[ep.Origin])))
					//    {
					//        ep.Origin = ATSNumbers.IndexOf(user.ATSNumbers[0]);
					//        Debug.Assert(ep.Origin >= 0);
					//    }

					//    ep.PriorityR2 = Math.Min(ep.PriorityR2, user.R2Priority);
					//}
				}
			}
		}

        //Verifica si en el panel de LC, se han configurado destinos ATS que tienen asociados destinos de telefonía LCEN
        //si es  así, se define un nombre único de grupo y se le asigna al Destino ATS y a los destinos de LCEN asociados.
        public bool PanelLC_AsociaDestinosATSyLCEN()
        {
            bool bCorrecto = true;
            const int TAM_MAX_GRUPO = 80; //Longitud máxima del nombre del grupo
            StringBuilder strGrupo=new StringBuilder();

            //Se verifica si en el panel de LC, se han configurado destinos ATS que tienen asociados destinos de telefonía LCEN
            try
            {
                if (null != _LcEndPointsATSWithLCEN && _LcEndPointsATSWithLCEN.Count>0)
                {
                    foreach (System.Collections.DictionaryEntry upd in _LcEndPointsATSWithLCEN)
                    {
                        strGrupo.Clear();

                        List<TLFEndPoint> objList = (List<TLFEndPoint>)_LcEndPointsATSWithLCEN[upd.Key];

                        string strDestinoAts = string.Empty;
                        string strDestionATsName = string.Empty;

                        strDestinoAts = upd.Key.ToString();

                        if (null != objList && objList.Count > 0)
                        {
                            int iNumTotal = objList.Count;

                             //Se obtiene el nombre del destino ATS
                            if (_LcEndPointsByName.ContainsKey(strDestinoAts))
                                strDestionATsName = _LcEndPointsByName[strDestinoAts].Name;
                            else
                                strDestionATsName = strDestinoAts;

                            strGrupo.Append(strDestionATsName);

                            //Obtenemos la lista de recursos LCEN asociados al destino ATS de la agrupación de sectores
                            for (int i = 0; i < iNumTotal; i++)
                            {
                                strGrupo.AppendFormat("##{0}",objList[i].Name);
                            }

                            //Una vez obtenido el nombre, se le asigna al grupo del destion ATS y a los recursos LCEN

                            if (!string.IsNullOrEmpty(strGrupo.ToString()))
                            {
                                if (strGrupo.Length > TAM_MAX_GRUPO)
                                {
                                    //Supera la longitud máxima del grupo, por lo que hay que generar un codigo automático
                                    strGrupo.Clear();
                                    strGrupo.AppendFormat("{0}##{1}##{2}", IdUsuario, strDestionATsName, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                                }

                                //Se le asigna al destino ATS el nombre del grupo
                                if (_LcEndPointsByName.ContainsKey(strDestinoAts))
                                {
                                    _LcEndPointsByName[strDestinoAts].Group = strGrupo.ToString();
                                }

                                //Se le asigna el mismo grupo a todos los recursos LCEN
                                for (int i = 0; i < iNumTotal; i++)
                                {
                                    objList[i].Group = strGrupo.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                Log(false, "PanelLC_AsociaDestinosATSyLCEN", "Error generico:" + objEx.Message.ToString());
                bCorrecto = false;
            }

            return bCorrecto;
        }


		public void MergeColateralsAg(UserInfo user, Sectorization sct)
		{
			if (user.AgEndPoints == null)
				return;
			
			foreach (TLFEndPoint userEP in user.AgEndPoints)
			{
				if (userEP == null)
				{
					continue;
				}

				string epId = userEP.Name + userEP.Prefix;
				TLFEndPoint ep = null;

				if (!_AgEndPointsByName.TryGetValue(epId, out ep))
				{
					UserInfo dstUser = null;
					ep = new TLFEndPoint(userEP);

					if (ep.Prefix == PrefixType.ATS)
					{
						//ep.Origin = user.ATSNumbers.Count > 0 ? ATSNumbers.IndexOf(user.ATSNumbers[0]) : Int32.MaxValue;
						ep.PriorityR2 = user.R2Priority;

						//Debug.Assert(ep.Origin >= 0);
					}
					else if (ep.Prefix == PrefixType.INT)
					{
						if (!sct.ListOfUsersByName.TryGetValue(ep.Name, out dstUser) || (dstUser.Top == null) || (dstUser.Top == Top) ||
							_InternalDstInOPE.ContainsKey(dstUser.Top.ToString() + ep.Prefix))
						{
							continue;
						}
					}
                    else if ((ep.Prefix == PrefixType.LCE) && (ep.Group != null))
                    {
                        ep.Priority = _LceGroupsPriority[ep.Group];
                        ep.Origin = ep.Group;

                        Indexer idx = _TlfIndex[1];
                        int pos = Array.FindIndex(LCEndPoints, (int)idx.Begin, (int)(idx.End - idx.Begin),
                        delegate(TLFEndPoint obj) { return ((obj != null) && (obj.Group == ep.Group)); });

                        if (pos >= 0)
                        {
                            for (ep.Position = idx.End; ep.Position < LCEndPoints.Length && LCEndPoints[ep.Position] != null; ep.Position++) ;
                            if (ep.Position >= LCEndPoints.Length) Array.Resize(ref LCEndPoints, (int)(ep.Position + 1));
                        }
                    }

					ep.Position = GetTlfPosition(ep);
					if (ep.Position > 0)
					{
						if (dstUser != null)
						{
							_InternalDstInOPE[dstUser.Top.ToString() + ep.Prefix] = null;
						}

						//TLFEndPoint replaceEp = AgEndPoints[ep.Position];
						//if ((replaceEp != null) && (replaceEp.Prefix == PrefixType.LCE) && (replaceEp.Group != 0))
						//{
						//    for (int i = _TlfIndex[1].End; i < TlfEndPoints.Length; i++)
						//    {
						//        if (TlfEndPoints[i].Group == replaceEp.Group)
						//        {
						//            TlfEndPoints[i] = null;
						//        }
						//    }
						//}

						AgEndPoints[ep.Position] = ep;
						_AgEndPointsByName[epId] = ep;
					}
				}
				else
				{
					ep.Priority = Math.Min(ep.Priority, userEP.Priority);

					if (ep.Prefix == PrefixType.ATS)
					{
                        //11/07/2018: Si en la agenda se configura un destino ATS, el campo ep.Origin almacena el sector del que procede la entrada en la agenda
                        //Por lo que no se debe actualizar con el menor de los numeros de abonados del sector en el caso de que la misma entrada esté 
                        //en varios sectores
                        /*
						if ((user.ATSNumbers.Count > 0) && (user.R2Priority < ep.PriorityR2) ||
							((user.R2Priority == ep.PriorityR2) && (user.ATSNumbers[0] < ATSNumbers[0])))
						{
                            ep.Origin = ATSNumbers[ATSNumbers.Count - 1].ToString();  // ATSNumbers está ordenada de mayor a menor -> Se coge el menor
                            
                            //ep.Origin = user.IdUsuario; // ATSNumbers.IndexOf(user.ATSNumbers[0]);
							//Debug.Assert(ep.Origin >= 0);
						}
                         */

						ep.PriorityR2 = Math.Min(ep.PriorityR2, user.R2Priority);
					}
				}
			}
		}

		public void MergeColateralsRd(UserInfo user, Sectorization sct)
		{
			if (user.RdEndPoints == null)
				return;
			
			foreach (RDEndPoint userEP in user.RdEndPoints)
			{
				if (userEP == null)
				{
					continue;
				}

				int pagina = (int)((userEP.Position-1) / user.NumFreqPagina);
				string epId = userEP.Frecuency;// +userEP.Position;
				RDEndPoint ep = null;

				if (!_RdEndPointsByName[pagina].TryGetValue(epId, out ep))
				{
					ep = new RDEndPoint(userEP);

                    ep.Position = GetRdPosition(ep, user.NumFreqPagina);

					if (ep.Position > 0)
					{
                        RdEndPoints[ep.Position] = ep;
						_RdEndPointsByName[pagina][epId] = ep;
					}
				}
				else
				{
                    if (ep.Position / NumFreqPagina == pagina)
                    {
                        ep.Priority = Math.Min(ep.Priority, userEP.Priority);
                        ep.SupervisionPortadora |= userEP.SupervisionPortadora;
                    }
                    else
                    {
                        // Si está en distinta página
                        ep.SupervisionPortadora |= userEP.SupervisionPortadora;
                    }
				}
			}
		}

		int GetTlfPosition(TLFEndPoint ep)
		{
			switch (ep.TipoAcceso)
			{
				case "DA":
					if (TlfEndPoints[ep.Position] != null)
					{
						ep.Position = -1;

						Indexer idx = _TlfIndex[0];

						for (; idx.Last < idx.End; idx.Last++)
						{
							if (TlfEndPoints[idx.Last] == null)
							{
								ep.Position = idx.Last++;
								break;
							}
						}

						if (ep.Position < 0)
						{
							for (int i = idx.Begin, priority = ep.Priority; i < idx.End; i++)
							{
								if (TlfEndPoints[i].Priority > priority)
								{
									priority = TlfEndPoints[i].Priority;
									ep.Position = i;
								}
							}
						}
					}
					break;
				case "IA":
					if (LCEndPoints[ep.Position] != null)
					{
						ep.Position = -1;

						Indexer idx = _TlfIndex[1];

						for (; idx.Last < idx.End; idx.Last++)
						{
							if (LCEndPoints[idx.Last] == null)
							{
								ep.Position = idx.Last++;
								break;
							}
						}

						if (ep.Position < 0)
						{
							for (int i = idx.Begin, priority = ep.Priority; i < idx.End; i++)
							{
								if (LCEndPoints[i].Priority > priority)
								{
									priority = LCEndPoints[i].Priority;
									ep.Position = i;
								}
							}
						}
					}
					break;
				case "AG":
					if (AgEndPoints[ep.Position] != null)
					{
						ep.Position = -1;

						Indexer idx = _TlfIndex[2];

						for (; idx.Last < idx.End; idx.Last++)
						{
							if (AgEndPoints[idx.Last] == null)
							{
								ep.Position = idx.Last++;
								break;
							}
						}

						if (ep.Position < 0)
						{
							for (int i = idx.Begin, priority = ep.Priority; i < idx.End; i++)
							{
								if (AgEndPoints[i].Priority > priority)
								{
									priority = AgEndPoints[i].Priority;
									ep.Position = i;
								}
							}
						}
					}
					break;
				default:
					break;
			}

			return ep.Position;
		}

		int GetRdPosition(RDEndPoint ep, uint piNumFreqPag)
		{
			if (RdEndPoints[ep.Position] != null)
			{
                int paginaRadio = (ep.Position-1) / (int)piNumFreqPag;

				ep.Position = -1;

				Indexer idx = _RdIndex[paginaRadio];

				for (; idx.Last < idx.End; idx.Last++)
				{
					if (RdEndPoints[idx.Last] == null)
					{
						ep.Position = idx.Last++;
						break;
					}
				}

				if (ep.Position < 0)
				{
					for (int i = idx.Begin, priority = ep.Priority; i < idx.End; i++)
					{
						if (RdEndPoints[i].Priority > priority)
						{
							priority = RdEndPoints[i].Priority;
							ep.Position = i;
						}
					}
				}
			}

			return ep.Position;
		}


        public void GeneraDestinosTelefoniaSectorizados(DbCommand cmd)
        {
            int iNumInternos = 0; //Almacena el número de registros insertados en la tabla internos
            int iNumExternos = 0; //Almacena el número de registros insertados en la tabla externos
            StringBuilder strMsg = new StringBuilder();

            if (TlfEndPoints != null)
            {
                //Se recorren el mapa con los destinos del panel de Telefonía del puesto de operador que son resultado de la sectorización
                //Para insertarlos en las tablas internos o externos en función del prefijo
                foreach (TLFEndPoint tf in TlfEndPoints)
                {
                    if (tf != null)
                    {
                        try
                        {
                            if (tf.Prefix == PrefixType.INT)
                            {
                                if (InsertaDestinoInterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, tf))
                                {
                                    iNumInternos++;
                                }
                                else
                                {
                                    strMsg.Clear();
                                    strMsg.AppendFormat("Sectorización={0}-Sector={1}. Error al insertar el destino interno del panel de telefonía:{2} - Tipo={3} TipoAcceso={4}.",IdSectorizacion,Literal, tf.IdDestino, tf.TipoDestino, tf.TipoAcceso);
                                    Log(true, "GeneraDestinosTelefoniaSectorizados", strMsg.ToString());
                                }
                            }
                            else
                            {
                                if (InsertaDestinoExterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, tf))
                                {
                                    iNumExternos++;
                                }
                                else
                                {
                                    strMsg.Clear();
                                    strMsg.AppendFormat("Sectorización={0}-Sector={1}. Error al insertar el destino externo del panel de telefonía:{2} - Tipo={3} TipoAcceso={4}.", IdSectorizacion, Literal, tf.IdDestino, tf.TipoDestino, tf.TipoAcceso);
                                    Log(true, "GeneraDestinosTelefoniaSectorizados", strMsg.ToString());
                                }
                            }
                        }
                        catch (System.Data.SqlClient.SqlException objEx)
                        {
                            strMsg.Clear();
                            strMsg.AppendFormat("Sectorización={0}-Sector={1}.Error al insertar el destino del panel de telefonía:{2} - Tipo={3} TipoAcceso={4}. Error: {5}", IdSectorizacion, Literal, tf.IdDestino, tf.TipoDestino, tf.TipoAcceso, objEx.Message.ToString());
                            Log(true, "GeneraDestinosTelefoniaSectorizados", strMsg.ToString());
                            strMsg.Clear();
                        }
                    }
                }
            }
        }

        /*
		public void GeneraDestinosLCSectorizados(DbCommand cmd)
		{
			if (LCEndPoints == null)
				return;

			foreach (TLFEndPoint tf in LCEndPoints)
			{
				if (tf != null)
				{
                    try
                    {
                        if (tf.Prefix == PrefixType.LCI)
                        {
                            cmd.CommandText = "INSERT INTO internos VALUES ('" + IdSistema + "'," +
                                                                            "'" + IdSectorizacion + "'," +
                                                                            "'" + IdNucleo + "'," +
                                                                            "'" + Literal + "'," +
                                                                            "'" + tf.IdDestino + "'," +
                                                                            (uint)tf.Prefix + "," +
                                                                            "'" + tf.TipoAcceso + "'," +
                                                                            tf.Position + "," +
                                                                            tf.Priority + "," +
                                                                            "'" + tf.Origin + "'," +
                                                                            tf.PriorityR2 + "," +
                                                                            "'" + tf.Literal + "')";
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySql.Data.MySqlClient.MySqlException e)
                            {
                                if (e.Number == 1062)
                                {
                                    // El registro ya existe, se elimina y lo volvemos a intenter
                                    cmd.CommandText = "DELETE FROM internos WHERE IdSistema='" + IdSistema + "' AND " +
                                                                                "IdSectorizacion='" + IdSectorizacion + "' AND " +
                                                                                "IdNucleo='" + IdNucleo + "' AND " +
                                                                                "IdSector='" + Literal + "' AND " +
                                                                                "IdDestino='" + tf.IdDestino + "' AND " +
                                                                                "IdPrefijo=" + (uint)tf.Prefix;
                                    cmd.ExecuteNonQuery();
                                    cmd.CommandText = "INSERT INTO internos VALUES ('" + IdSistema + "'," +
                                                                                    "'" + IdSectorizacion + "'," +
                                                                                    "'" + IdNucleo + "'," +
                                                                                    "'" + Literal + "'," +
                                                                                    "'" + tf.IdDestino + "'," +
                                                                                    (uint)tf.Prefix + "," +
                                                                                    "'" + tf.TipoAcceso + "'," +
                                                                                    tf.Position + "," +
                                                                                    tf.Priority + "," +
                                                                                    "'" + tf.Origin + "'," +
                                                                                    tf.PriorityR2 + "," +
                                                                                    "'" + tf.Literal + "')";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('internos') ";
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = "INSERT INTO externos VALUES ('" + IdSistema + "'," +
                                                                            "'" + IdSectorizacion + "'," +
                                                                            "'" + IdNucleo + "'," +
                                                                            "'" + Literal + "'," +
                                                                            "'" + tf.IdDestino + "'," +
                                                                            (uint)tf.Prefix + "," +
                                                                            "'" + tf.TipoAcceso + "'," +
                                                                            tf.Position + "," +
                                                                            tf.Priority + "," +
                                                                            "'" + tf.Origin + "'," +
                                                                            tf.PriorityR2 + "," +
                                                                            "'" + tf.Literal + "'," +
                                                                            "'" + tf.Group + "')";
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySql.Data.MySqlClient.MySqlException e)
                            {
                                if (e.Number == 1062)
                                {
                                    // El registro ya existe, se elimina y lo volvemos a intentar
                                    cmd.CommandText = "DELETE FROM externos WHERE IdSistema='" + IdSistema + "' AND " +
                                                                                "IdSectorizacion='" + IdSectorizacion + "' AND " +
                                                                                "IdNucleo='" + IdNucleo + "' AND " +
                                                                                "IdSector='" + Literal + "' AND " +
                                                                                "IdDestino='" + tf.IdDestino + "' AND " +
                                                                                "IdPrefijo=" + (uint)tf.Prefix;
                                    cmd.ExecuteNonQuery();
                                    cmd.CommandText = "INSERT INTO externos VALUES ('" + IdSistema + "'," +
                                                                                    "'" + IdSectorizacion + "'," +
                                                                                    "'" + IdNucleo + "'," +
                                                                                    "'" + Literal + "'," +
                                                                                    "'" + tf.IdDestino + "'," +
                                                                                    (uint)tf.Prefix + "," +
                                                                                    "'" + tf.TipoAcceso + "'," +
                                                                                    tf.Position + "," +
                                                                                    tf.Priority + "," +
                                                                                    "'" + tf.Origin + "'," +
                                                                                    tf.PriorityR2 + "," +
                                                                                    "'" + tf.Literal + "'," +
                                                                                    "'" + tf.Group + "')";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('externos') ";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (System.Data.SqlClient.SqlException )
                    {

                    }
                }
			}
		}
*/
        //Inserta un destino interno en la tabla Internos. Si el registro existe lo actualiza
        private bool InsertaDestinoInterno(DbCommand cmd, string strIdSistema, string strIdSectorizacion, string strIdNucleo, string strNombreSector, TLFEndPoint objDest)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            StringBuilder strMsg= new StringBuilder();

            if (null!=objDest)
            {
                try
                {
                    //Se compone la sentencia Insert
                    strSentencia.Append("INSERT INTO internos (IdSistema,IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,TipoAcceso,PosHMI,Prioridad,OrigenR2,PrioridadSIP,Literal)");
                    strSentencia.AppendFormat(" VALUES('{0}','{1}','{2}','{3}','{4}',{5},'{6}',{7},{8},'{9}',{10},'{11}') ", strIdSistema, 
                                           strIdSectorizacion, strIdNucleo, strNombreSector, objDest.IdDestino,(uint)objDest.Prefix,
                                           objDest.TipoAcceso,objDest.Position ,objDest.Priority,objDest.Origin,objDest.PriorityR2,objDest.Literal);

                    cmd.CommandText = strSentencia.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        bCorrecto = true;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException mysqlEx)
                    {
                        if (mysqlEx.Number == 1062)
                        {
                            // Si el registro existe en la BD se actualiza 
                            StringBuilder strUpdate = new StringBuilder();

                            //Se compone la sentencia Update
                            strUpdate.AppendFormat("UPDATE internos SET PosHMI={0},Prioridad={1},OrigenR2='{2}',PrioridadSIP={3},Literal='{4}'", objDest.Position, objDest.Priority, objDest.Origin, objDest.PriorityR2, objDest.Literal);
                            strUpdate.AppendFormat(" WHERE IdSistema='{0}' AND IdSectorizacion='{1}' AND IdNucleo='{2}' AND IdSector='{3}' AND IdDestino='{4}' AND IdPrefijo={5} AND TipoAcceso='{6}'", strIdSistema,
                                           strIdSectorizacion, strIdNucleo, strNombreSector, objDest.IdDestino, (uint)objDest.Prefix, objDest.TipoAcceso);

                            cmd.CommandText = strUpdate.ToString();

                            try
                            {
                                cmd.ExecuteNonQuery();
                                bCorrecto = true;
                            }
                            catch (MySql.Data.MySqlClient.MySqlException mySqlExUpd)
                            {
                                strMsg.AppendFormat("Error al actualizar el destino interno ({0}:{1}). Sentencia:{2}", mySqlExUpd.Number, mySqlExUpd.Message, strUpdate.ToString());
                                Log(true, "InsertaDestinoInterno", strMsg.ToString());
                                strMsg.Clear();
                            }

                            strUpdate.Clear();
                        }
                        else
                        {
                            strMsg.AppendFormat("Error al insertar el destino interno ({0}:{1}) al ejecutar la sentencia:{2}", mysqlEx.Number, mysqlEx.Message, strSentencia.ToString());
                            Log(true, "InsertaDestinoInterno", strMsg.ToString());
                            strMsg.Clear();
                        }
                    }
                }

                catch (Exception objEx)
                {
                    strMsg.AppendFormat("Error al insertar el destino interno:{0} - Tipo={1} TipoAcceso={2}. Error: {3}", objDest.Name.ToString(), objDest.TipoDestino, objDest.TipoAcceso, objEx.Message.ToString());
                    Log(true, "InsertaDestinoInterno", strMsg.ToString());
                    bCorrecto = false;
                    strMsg.Clear();
                }

                strSentencia.Clear();
            }

            return bCorrecto;
        }

        //Inserta o actualiza el registro de TablasModificadas correspondiente a la tabla que se le pasa como parámetro
        bool InsertaTablasModificadas(DbCommand cmd, string strNombreTabla)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia= new StringBuilder();

            if (!string.IsNullOrEmpty(strNombreTabla))
            {
                try
                {
                    strSentencia.AppendFormat("REPLACE INTO TablasModificadas (IdTabla) VALUES ('{0}') ", strNombreTabla);
                    cmd.CommandText = strSentencia.ToString();
                    cmd.ExecuteNonQuery();
                    bCorrecto = true;
                }
                catch (MySql.Data.MySqlClient.MySqlException mysqlEx)
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendFormat("Error al ejecutar la sentencia:{0}. Error:{1} - {2} ", strSentencia, mysqlEx.Number,mysqlEx.Message.ToString());
                    Log(true, "InsertaTablasModificadas", strMsg.ToString());
                    strMsg.Clear();
                }
            }

            return bCorrecto;
        }

        //Inserta un destino externo en la tabla Externos. Si el registro existe lo actualiza
        private bool InsertaDestinoExterno(DbCommand cmd, string strIdSistema, string strIdSectorizacion, string strIdNucleo, string strNombreSector, TLFEndPoint objDest)
        {
            bool bCorrecto = false;
            StringBuilder strSentencia = new StringBuilder();
            StringBuilder strMsg = new StringBuilder();

            if (null != objDest)
            {
                try
                {
                    //Se compone la sentencia Insert
                    strSentencia.Append("INSERT INTO externos (IdSistema,IdSectorizacion,IdNucleo,IdSector,IdDestino,IdPrefijo,TipoAcceso,PosHMI,Prioridad,OrigenR2,PrioridadSIP,Literal,Grupo)");
                    strSentencia.AppendFormat(" VALUES('{0}','{1}','{2}','{3}','{4}',{5},'{6}',{7},{8},'{9}',{10},'{11}','{12}') ", strIdSistema,
                                           strIdSectorizacion, strIdNucleo, strNombreSector, objDest.IdDestino, (uint)objDest.Prefix,
                                           objDest.TipoAcceso, objDest.Position, objDest.Priority, objDest.Origin, objDest.PriorityR2, objDest.Literal,objDest.Group);

                    cmd.CommandText = strSentencia.ToString();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        bCorrecto = true;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException mysqlEx)
                    {
                        if (mysqlEx.Number == 1062)
                        {
                            // Si el registro existe en la BD se actualiza 
                            StringBuilder strUpdate = new StringBuilder();

                            //Se compone la sentencia Update
                            strUpdate.AppendFormat("UPDATE externos SET PosHMI={0},Prioridad={1},OrigenR2='{2}',PrioridadSIP={3},Literal='{4}',Grupo='{5}'", objDest.Position, objDest.Priority, objDest.Origin, objDest.PriorityR2, objDest.Literal,objDest.Group);
                            strUpdate.AppendFormat(" WHERE IdSistema='{0}' AND IdSectorizacion='{1}' AND IdNucleo='{2}' AND IdSector='{3}' AND IdDestino='{4}' AND IdPrefijo={5} AND TipoAcceso='{6}'", 
                                                    strIdSistema,strIdSectorizacion, strIdNucleo, strNombreSector, objDest.IdDestino, (uint)objDest.Prefix, objDest.TipoAcceso);
                            cmd.CommandText = strUpdate.ToString();

                            try
                            {
                                cmd.ExecuteNonQuery();
                                bCorrecto = true;
                            }
                            catch (MySql.Data.MySqlClient.MySqlException mySqlExUpd)
                            {
                                strMsg.AppendFormat("Error al actualizar el destino externo ({0}:{1}). Sentencia:{2}", mySqlExUpd.Number, mySqlExUpd.Message, strUpdate.ToString());
                                Log(true, "InsertaDestinoExterno", strMsg.ToString());
                            }

                            strUpdate.Clear();
                        }
                        else
                        {
                            strMsg.AppendFormat("Error al insertar el destino externo ({0}:{1}).Sentencia: {2}", mysqlEx.Number, mysqlEx.Message, strSentencia.ToString());
                            Log(true, "InsertaDestinoExterno", strMsg.ToString());
                        }
                    }
                }

                catch (Exception objEx)
                {
                    strMsg.AppendFormat("Error al insertar el destino interno:{0} - Tipo={1} TipoAcceso={2}. Error: {3}", objDest.Name.ToString(), objDest.TipoDestino, objDest.TipoAcceso, objEx.Message.ToString());
                    Log(true, "InsertaDestinoInterno", strMsg.ToString());
                    bCorrecto = false;
                    
                }

                strSentencia.Clear();
                if (strMsg.Length>0)
                    strMsg.Clear();
            }

            return bCorrecto;
        }

        //Inserta en la BD los Destinos del panel de Línea caliente que son resultado de la sectorización
        public void GeneraDestinosLCSectorizados(DbCommand cmd)
        {
            int iNumInternos=0; //Almacena el número de registros insertados en la tabla internos
            int iNumExternos=0; //Almacena el número de registros insertados en la tabla externos
            StringBuilder strMsg = new StringBuilder();

            if (null != LCEndPoints)
            {
                //Se recorren el mapa con los destinos del panel de LC del puesto de operador, que son resultado de la sectorización
                foreach (TLFEndPoint tf in LCEndPoints)
                {
                    //Por cada destino configurado en el panel de línea caliente
                    if (null!= tf)
                    {
                        try
                        {
                            //Si es un destino interno
                            if (tf.Prefix == PrefixType.LCI)
                            {
                                if (InsertaDestinoInterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, tf))
                                {
                                    iNumInternos++;
                                }
                            }
                            else
                            {
                                if (InsertaDestinoExterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, tf))
                                {
                                    iNumExternos++;

                                    if (tf.Prefix == PrefixType.ATS)
                                    {
                                        //Se verifica si el destino ATS tiene asociados recursos LCEN para insertarlos
                                        if (null != _LcEndPointsATSWithLCEN && _LcEndPointsATSWithLCEN.ContainsKey(tf.IdDestino + tf.Prefix))
                                        {
                                            List<TLFEndPoint> objList = (List<TLFEndPoint>)_LcEndPointsATSWithLCEN[tf.IdDestino + tf.Prefix];

                                            if (null != objList && objList.Count > 0)
                                            {
                                                //Insertamos la lista de recursos LCEN asociados al destino ATS de la agrupación de sectores
                                                for (int i = 0; i < objList.Count; i++)
                                                {
                                                    if (InsertaDestinoExterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, objList[i]))
                                                        iNumExternos++;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    strMsg.Clear();
                                    strMsg.AppendFormat("Sectorización={0}-Sector={1}. Error al insertar el destino externo del panel LC:{2} - Tipo={3} TipoAcceso={4}.", IdSectorizacion, Literal, tf.IdDestino, tf.TipoDestino, tf.TipoAcceso);
                                    Log(true, "GeneraDestinosTelefoniaSectorizados", strMsg.ToString());
                                }
                            }
                        }
                        catch (Exception objEx)
                        {
                            strMsg.Clear();
                            strMsg.AppendFormat("Sectorización={0}-Sector={1}.Error al insertar el destino en el panel de LC:{2} - Tipo={3} TipoAcceso={4}. Error: {5}", IdSectorizacion, Literal, tf.IdDestino, tf.TipoDestino, tf.TipoAcceso, objEx.Message.ToString());
                            Log(true, "GeneraDestinosLCSectorizados", strMsg.ToString());
                        }
                    }
                }

                if (iNumExternos > 0)
                {
                    //Si se ha insertado algún destino externo, se inserta o actualiza el registro en TablasModificadas
                    InsertaTablasModificadas(cmd, "externos");
                }

                if (iNumInternos > 0)
                {
                    //Si se ha insertado algún destino interno, se inserta o actualiza el registro en TablasModificadas con idTabla=internos
                    InsertaTablasModificadas(cmd, "internos");
                }
            }
        }


		public void GeneraDestinosAgendaSectorizados(DbCommand cmd)
		{
            int iNumInternos = 0; //Almacena el número de destinos internos insertados
            int iNumExternos = 0; //Almacena el número de destinos externos insertados

            if (AgEndPoints != null)
            {
                //Se recorren el mapa con los destinos de la agenda del puesto de operador que son resultado de la sectorización
                foreach (TLFEndPoint tf in AgEndPoints)
                {
                    if (tf != null)
                    {
                        try
                        {
                            if (tf.Prefix == PrefixType.INT)
                            {
                                if (InsertaDestinoInterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, tf))
                                {
                                    iNumInternos++;
                                }
                            }
                            else
                            {
                                if (InsertaDestinoExterno(cmd, IdSistema, IdSectorizacion, IdNucleo, Literal, tf))
                                {
                                    iNumExternos++;
                                }
                            }
                        }
                        catch (Exception objEx)
                        {
                            StringBuilder strMsg = new StringBuilder();
                            strMsg.AppendFormat("Error al insertar en la Agenda:{0} - Tipo={1} TipoAcceso={2}. Error: {3}", tf.IdDestino, tf.TipoDestino, tf.TipoAcceso, objEx.Message.ToString());
                            Log(true, "GeneraDestinosAgendaSectorizados", strMsg.ToString());
                        }
                    }
                }

                if (iNumExternos > 0)
                {
                    //Si se ha insertado algún destino externo, se inserta o actualiza el registro en TablasModificadas
                    InsertaTablasModificadas(cmd, "externos");
                }

                if (iNumInternos > 0)
                {
                    //Si se ha insertado algún destino interno, se inserta o actualiza el registro en TablasModificadas con idTabla=internos
                    InsertaTablasModificadas(cmd, "internos");
                }
            }
        }

		public void GeneraDestinosRadioSectorizados(DbCommand cmd)
		{
            const int NUM_TAB = 3;
            int[] iNumTabMod = new int[NUM_TAB];        //Almacena el número de registros insertados o modificados en cada tabla de la BD
            const int TAB_RADIO=0;                     //Indice correspondiente a la tabla radio
            const int TAB_ESTADORECURSOS = 1;          //Indice correspondiente a la tabla EstadoRecursos
            const int TAB_ESTADOALTAVOCES = 2;         //Indice correspondiente a la tabla EstadoAltavoces
            string strTabMod = string.Empty;
            uint i = 0;
            StringBuilder strSentencia = new StringBuilder();

            if (RdEndPoints != null)
            {
                //Se inicializa el vector
                for (i = 0; i < iNumTabMod.Length; i++)
                {
                    iNumTabMod[i] = 0;
                }

                //Para cada destino radio del panel radio
                foreach (RDEndPoint rd in RdEndPoints)
                {
                    try
                    {
                        if (rd != null)
                        {
                            strSentencia.Clear();

                            //El campo Cascos se inserta a null
                            strSentencia.Append("INSERT INTO radio (IdSistema,IdSectorizacion,IdNucleo,IdSector,PosHMI,IdDestino,Prioridad,PrioridadSIP,ModoOperacion,Cascos,Literal,SupervisionPortadora)");

                            strSentencia.AppendFormat(" VALUES ('{0}','{1}','{2}','{3}',{4},'{5}',{6},{7},'{8}',null,'{9}',{10})", IdSistema, IdSectorizacion, IdNucleo, Literal,
                                                       rd.Position, rd.Frecuency,
                                                       rd.Priority,
                                                       rd.PrioridadSIP, rd.EstadoAsignacion, rd.Literal, rd.SupervisionPortadora);


                            cmd.CommandText = strSentencia.ToString();

                            try
                            {
                                cmd.ExecuteNonQuery();
                                iNumTabMod[TAB_RADIO]++;
                            }
                            catch (MySql.Data.MySqlClient.MySqlException e)
                            {
                                if (e.Number == 1062)
                                {
                                    // El registro ya existe, se elimina y lo volvemos a intenter
                                    cmd.CommandText = "DELETE FROM radio WHERE IdSistema='" + IdSistema + "' AND " +
                                                                                "IdSectorizacion='" + IdSectorizacion + "' AND " +
                                                                                "IdNucleo='" + IdNucleo + "' AND " +
                                                                                "IdSector='" + Literal + "' AND " +
                                                                                "PosHMI='" + rd.Position;
                                    cmd.ExecuteNonQuery();
                                    cmd.CommandText = strSentencia.ToString();

                                    cmd.ExecuteNonQuery();
                                    iNumTabMod[TAB_RADIO]++;
                                }
                            }

                            // Insertar estado recurso sectorizado
                            foreach (string recurso in rd.EstadoRecursos.Keys)
                            {
                                cmd.CommandText = "REPLACE INTO EstadoRecursos SET IdDestino='" + rd.Frecuency + "'," +
                                                        "PosHMI=" + (uint)rd.Position + "," +
                                                        "IdSector='" + Literal + "'," +
                                                        "IdNucleo='" + IdNucleo + "'," +
                                                        "IdSectorizacion='" + IdSectorizacion + "'," +
                                                        "IdSistema='" + IdSistema + "'," +
                                                        "IdRecurso='" + recurso + "'," +
                                                        "Estado='" + rd.EstadoRecursos[recurso] + "'";
                                cmd.ExecuteNonQuery();

                                iNumTabMod[TAB_ESTADORECURSOS]++;
                            }

                            // Insertar la asignación sectorizada de altavoces 
                            i = 1;
                            foreach (string estado in rd.EstadoAltavoces)
                            {
                                cmd.CommandText = "REPLACE INTO EstadoAltavoces SET IdDestino='" + rd.Frecuency + "'," +
                                                        "PosHMI=" + (uint)rd.Position + "," +
                                                        "IdSector='" + Literal + "'," +
                                                        "IdNucleo='" + IdNucleo + "'," +
                                                        "IdSectorizacion='" + IdSectorizacion + "'," +
                                                        "IdSistema='" + IdSistema + "'," +
                                                        "NumAltavoz=" + i++ + "," +
                                                        "Estado='" + estado + "'";
                                cmd.ExecuteNonQuery();

                                iNumTabMod[TAB_ESTADOALTAVOCES]++;
                            }
                        }
                    }
                    catch (System.Data.Common.DbException exp)
                    {
                        StringBuilder strMsg = new StringBuilder();

                        strMsg.AppendFormat("Sectorización={0}-Sector={1}. Error al insertar el destino radio :{2} - Posicion={3}.Error:{4}", IdSectorizacion, Literal, rd.Frecuency, rd.Position,exp.Message.ToString());
                        Log(true, "GeneraDestinosRadioSectorizados", strMsg.ToString());
                    }
                }

                //Se actualiza la tabla TablasModificadas
                for (i = 0; i < iNumTabMod.Length; i++)
                {
                    if (iNumTabMod[i] > 0)
                    {
                        switch (i)
                        {
                            case TAB_RADIO:                    //Tabla radio
                                InsertaTablasModificadas(cmd, "radio");
                                break;
                            case TAB_ESTADORECURSOS:          //Tabla EstadoRecursos
                                InsertaTablasModificadas(cmd, "EstadoRecursos");
                                break;
                            case TAB_ESTADOALTAVOCES:         //Tabla EstadoAltavoces
                                InsertaTablasModificadas(cmd, "EstadoAltavoces");
                                break;
                        }
                    }
                }
            }
		}
		/*
		internal void GeneraEstadoRecursos(DbCommand cmd)
		{
			cmd.CommandText = "DELETE FROM EstadoRecursos WHERE IdSector='" + Literal + "' AND " +
												"IdNucleo='" + IdNucleo + "' AND " +
												"IdSectorizacion='" + IdSectorizacion + "' AND " +
												"IdSistema='" + IdSistema + "'";
			cmd.ExecuteNonQuery();

			foreach (RDEndPoint rd in RdEndPoints)
			{
				if (rd != null)
				{
					foreach (string recurso in rd.EstadoRecursos.Keys)
					{
						cmd.CommandText = "INSERT INTO EstadoRecursos SET IdDestino='" + rd.Frecuency + "'," +
												"PosHMI=" + (uint)rd.Position + "," +
												"IdSector='" + Literal + "'," +
												"IdNucleo='" + IdNucleo + "'," +
												"IdSectorizacion='" + IdSectorizacion + "'," +
												"IdSistema='" + IdSistema + "'," +
												"IdRecurso='" + recurso + "'," +
												"Estado='" + rd.EstadoRecursos[recurso] + "'";
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

		internal void GeneraEstadoAltavoces(DbCommand cmd)
		{
			cmd.CommandText = "DELETE FROM EstadoAltavoces WHERE IdSector='" + Literal + "' AND " +
												"IdNucleo='" + IdNucleo + "' AND " +
												"IdSectorizacion='" + IdSectorizacion + "' AND " +
												"IdSistema='" + IdSistema + "'";
			cmd.ExecuteNonQuery();

			foreach (RDEndPoint rd in RdEndPoints)
			{
				if (rd != null)
				{
					uint i = 1;
					foreach (string estado in rd.EstadoAltavoces)
					{
						cmd.CommandText = "INSERT INTO EstadoAltavoces SET IdDestino='" + rd.Frecuency + "'," +
												"PosHMI=" + (uint)rd.Position + "," +
												"IdSector='" + Literal + "'," +
												"IdNucleo='" + IdNucleo + "'," +
												"IdSectorizacion='" + IdSectorizacion + "'," +
												"IdSistema='" + IdSistema + "'," +
												"NumAltavoz=" + i++ + "," +
												"Estado='" + estado + "'";
						cmd.ExecuteNonQuery();
					}
				}
			}
		}
		 */

		internal void GeneraParametrosSector(DbCommand cmd)
		{
            StringBuilder strCadena=new StringBuilder();

            // Eliminar los parámetros del sector si los tuviera
            cmd.CommandText = "DELETE FROM ParametrosSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "' AND IdSector='" + Literal + "'";
            cmd.ExecuteNonQuery();

            // Generar los nuevos parámetros
            /*
            cmd.CommandText = "INSERT INTO ParametrosSector SET " +
                        "IdSistema='" + IdSistema + "'," +
                        "IdNucleo='" + IdNucleo + "'," +
                        "IdSector='" + Literal + "'," +
                        "NumLLamadasEntrantesIDA=" + NumLLamadasEntrantesIDA + "," +
                        "NumLlamadasEnIDA=" + NumLlamadasEnIDA + "," +
                        "NumFreqPagina=" + NumFreqPagina + "," +
                        "NumPagFreq=" + NumPagFreq + "," +
                        "NumDestinosInternosPag=" + NumDestinosInternosPag + "," +
                        "NumPagDestinosInt=" + NumPagDestinosInt + "," +
                        "Intrusion=" + Intrusion + "," +
                        "Intruido=" + Intruido + "," +
                        "KeepAlivePeriod=" + KAP + "," +
                        "KeepAliveMultiplier=" + KAM + "," +
                        "NumEnlacesAI=" + NumEnlacesAI;
             */
            // En la generación de parámetros se añade la gestión del campo GrabaciónED137
            strCadena.Append("INSERT INTO ParametrosSector (IdSistema,IdNucleo,IdSector,NumLlamadasEntrantesIDA,NumLlamadasEnIDA,NumFreqPagina,NumPagFreq,");
            strCadena.Append("NumDestinosInternosPag,NumPagDestinosInt,Intrusion,Intruido,KeepAlivePeriod,KeepAliveMultiplier,NumEnlacesAI,GrabacionEd137)");
            strCadena.AppendFormat(" VALUES ('{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14} )",IdSistema,IdNucleo,Literal,NumLLamadasEntrantesIDA,
                                    NumLlamadasEnIDA,NumFreqPagina,NumPagFreq,NumDestinosInternosPag,NumPagDestinosInt,
                                    Intrusion,Intruido,KAP,KAM,NumEnlacesAI,GrabacionEd137);

            cmd.CommandText = strCadena.ToString();
            cmd.ExecuteNonQuery();

            strCadena.Clear();

            cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('ParametrosSector') ";
            cmd.ExecuteNonQuery();
        }

        internal void GeneraTeclasSector(DbCommand cmd)
        {
            // Eliminar las teclas del sector si los tuviera
            cmd.CommandText = "DELETE FROM TeclasSector WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "' AND IdSector='" + Literal + "'";
            cmd.ExecuteNonQuery();

            // Generar los nuevos parámetros
            cmd.CommandText = "INSERT INTO TeclasSector SET " +
                        "IdSistema='" + IdSistema + "'," +
                        "IdNucleo='" + IdNucleo + "'," +
                        "IdSector='" + Literal + "'," +
                        "TransConConsultaPrev=" + TransConConsultaPrev + "," +
                        "TransDirecta=" + TransDirecta + "," +
                        "Conferencia=" + Conferencia + "," +
                        "Escucha=" + Escucha + "," +
                        "Retener=" + Retener + "," +
                        "Captura=" + Captura + "," +
                        "Redireccion=" + Redireccion + "," +
                        "RepeticionUltLlamada=" + RepeticionUltLlamada + "," +
                        "RellamadaAut=" + RellamadaAut + "," +
                        "TeclaPrioridad=" + TeclaPrioridad + "," +
                        "Tecla55mas1=" + Tecla55mas1 + "," +
                        "Monitoring=" + Monitoring + "," +
                        "CoordinadorTF=" + CoordinadorTF + "," +
                        "CoordinadorRD=" + CoordinadorRD + "," +
                        "IntegracionRDTF=" + IntegracionRDTF + "," +
                        "LlamadaSelectiva=" + LlamadaSelectiva + "," +
                        "GrupoBSS=" + GrupoBSS + "," +
                        "LTT=" + LTT + "," +
                        "SayAgain=" + SayAgain + "," +
                        "InhabilitacionRedirec=" + InhabilitacionRedirec + "," +
                        "Glp=" + Glp;
                        
            cmd.ExecuteNonQuery();

            cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('TeclasSector') ";
            cmd.ExecuteNonQuery();
        }

		internal void GeneraNumerosAbonado(DbCommand cmd)
		{
			// Eliminar los parámetros del sector si los tuviera
			cmd.CommandText = "DELETE FROM UsuariosAbonados WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "' AND IdSector='" + Literal + "'";
			cmd.ExecuteNonQuery();

			// Generar los números de abonados != de ATS
			foreach (StAbonado ab in PublicNumbers)
			{
				cmd.CommandText = "INSERT INTO UsuariosAbonados SET " +
							"IdSistema='" + IdSistema + "'," +
							"IdNucleo='" + IdNucleo + "'," +
							"IdSector='" + Literal + "'," +
							"IdPrefijo=" + ab.Prefijo + "," +
							"IdAbonado='" + ab.Numero + "'";
				cmd.ExecuteNonQuery();
			}

			// Generar los números de abonado ATS
			foreach (int n in ATSNumbers)
			{
				cmd.CommandText = "INSERT INTO UsuariosAbonados SET " +
							"IdSistema='" + IdSistema + "'," +
							"IdNucleo='" + IdNucleo + "'," +
							"IdSector='" + Literal + "'," +
							"IdPrefijo=" + 3 + "," +
							"IdAbonado='" + n.ToString() + "'";
				cmd.ExecuteNonQuery();
			}

            if (PublicNumbers.Count > 0 || ATSNumbers.Count > 0)
            {
                cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('UsuariosAbonados') ";
                cmd.ExecuteNonQuery();
            }

		}

        internal void GeneraPermisosRedesSector(DbCommand cmd)
        {
            // Eliminar las teclas del sector si los tuviera
            cmd.CommandText = "DELETE FROM PermisosRedes WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + IdNucleo + "' AND IdSector='" + Literal + "'";
            cmd.ExecuteNonQuery();

            // Generar los nuevos parámetros
            foreach (System.Collections.Generic.KeyValuePair<string, PermisoRed> p in PermisoRedes)
            {
                cmd.CommandText = "INSERT INTO PermisosRedes SET " +
                            "IdSistema='" + IdSistema + "'," +
                            "IdRed='" + p.Key + "'," +
                            "IdSector='" + Literal + "'," +
                            "IdNucleo='" + IdNucleo + "'," +
                            "Llamar='" + p.Value.Llamar + "'," +
                            "Recibir='" + p.Value.Recibir + "'";

                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('PermisosRedes') ";
            cmd.ExecuteNonQuery();
        }

        void Log(bool isError, string from, string msg, params object[] par)
        {
            StringBuilder message = new StringBuilder();

            message.AppendFormat("[{0}.{1}]: {2}", "Sectorization", from, msg);

            if (isError)
                NLog.LogManager.GetLogger("Sectorization").Error(message.ToString(), par);
            else
                NLog.LogManager.GetLogger("Sectorization").Info(message.ToString(), par);

            message.Clear();
        }
    }

    /// <summary>
    /// 
    /// </summary>
	internal class SectorInfo
	{
		public string IdSistema;

		public UcsInfo Ucs;
		public int IdSector;

		public string IdNucleo;
		public string Name;
		public UserInfo UserE;
		public UserInfo UserP;
		public string Type;

		public SectorInfo(UcsInfo ucs, int idSector)
		{
			Ucs = ucs;
			IdSector = idSector;
		}
	}

    /// <summary>
    /// 
    /// </summary>
	internal class UcsInfo
	{
		string IdSistema;
		public string TopE;
		public string TopP;
		public UserInfo UserE;
		public UserInfo UserP;
		public SectorGroupInfo Group;

		public List<SectorInfo> Sectors = new List<SectorInfo>();

		public UcsInfo(string idSistema)
		{
			IdSistema = idSistema;
		}

		public bool IsMultiOPE
		{
			get { return (TopP != null); }
		}

		public void MergeUsers(Sectorization sct, DbCommand cmd, char extE, char extP)
		{
			int lenDA = 0;
			UserE = new UserInfo(Sectors[0].UserE);
			UserP = IsMultiOPE ? new UserInfo(Sectors[0].UserP) : null;

            //Se obtiene la configuración del sector configurado en la UCS
			UserE.CreaUser(Sectors[0].UserE);

			if (IsMultiOPE)
			{
				UserP.CreaUser(Sectors[0].UserP);
			}

			foreach (SectorGroupInfo gr in sct.ListOfGroups.Values)
			{
				if ((gr.Sectors.Count == Sectors.Count) &&
					Sectors.TrueForAll(delegate(SectorInfo obj) { return gr.Sectors.Contains(obj.IdSector); }))
				{
					Group = gr;
					break;
				}
			}

			cmd.CommandText = "SELECT TamLiteralDA " +
			                       "FROM Sistema " +
			                       "WHERE IdSistema='" + IdSistema + "'";
			using (DbDataReader dr = cmd.ExecuteReader())
			{
				if (dr.Read())
				{
					lenDA = (int)(uint)dr["TamLiteralDA"];
				}
				else
				{
					throw new SectorizationException(SectorizationResult.Error, Resources.EmptySectError);
				}
			}

			int maxlen = IsMultiOPE ? (lenDA-2) : lenDA;
			StringBuilder name = new StringBuilder(); 
			int len = Math.Max(1, maxlen / Sectors.Count);
			bool firstUserE = true;
			bool firstUserP = true;

			foreach (SectorInfo sector in Sectors)
			{
				if (Group == null)
				{
					if (sector.IdNucleo != Sectors[0].IdNucleo)
					{
						throw new SectorizationException(SectorizationResult.MultiNucleoWithoutGroup, Resources.MultiNucleoSectError);
					}
					name.Append(sector.Name.Substring(0, Math.Min(len, sector.Name.Length)));
					// name = sector.Name;
				}
				else if (Group.Especial)
				{
					Array.Resize(ref sector.UserE.TlfEndPoints, 0);
					if (firstUserE)
					{
		                sector.UserE.TlfEndPoints = Group.TlfEndPointsE;
				        firstUserE = false;
					}
				}

				UserE.MergeUser(sector.UserE);

				Debug.Assert((sector.UserP == null) || IsMultiOPE);
				if (IsMultiOPE)
				{
					if ((Group != null) && Group.Especial)
					{
						Array.Resize(ref sector.UserP.TlfEndPoints, 0);
						if (firstUserP)
						{
							sector.UserP.TlfEndPoints = Group.TlfEndPointsP;
							firstUserP = false;
						}
					}

					UserP.MergeUser(sector.UserP);
				}
			}

			UserE.Literal = (Group != null) ? Group.Name : name.ToString();
			if (UserE.Literal.Length > maxlen) 
				UserE.Literal = UserE.Literal.Substring(0, maxlen);
			

			try
			{
				bool insertar = false;
				cmd.CommandText = "SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + Sectors[0].UserE.IdNucleo +
									"' AND IdSector='" + UserE.Literal + "'";
				using (DbDataReader dr = cmd.ExecuteReader())
				{
					insertar = !dr.HasRows;
				}

				if (insertar)
				{
                    cmd.CommandText = "INSERT INTO Sectores (IdSistema,IdNucleo,IdSector,SectorSimple,Tipo,TipoPosicion,PrioridadR2," +
                                                            "NumSacta)" +
                                    " VALUES ('" + IdSistema + "','" +
                                                 Sectors[0].UserE.IdNucleo + "','" +
                                                 UserE.Literal + "'," +
                                                 false + ",'" +
                                                 Sectors[0].UserE.Sector.Type + "','" +
                                                 sct.CaracterEjecutivo.ToString() + "'," +
                                                 UserE.R2Priority + "," +
                                                 Sectors[0].UserE.Sector.IdSector + ")";
					cmd.ExecuteNonQuery();
                    cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('Sectores') ";
                    cmd.ExecuteNonQuery();


					// Crear SectoresSector
					if (sct.Name == "SACTA")
					{
						int dominante = 0;
						foreach (SectorInfo s in Sectors)
						{
							cmd.CommandText = "INSERT INTO SectoresSector (IdSistema,IdNucleo,IdSector,IdSectorOriginal,EsDominante)" +
									" VALUES ('" + IdSistema + "','" +
												 s.UserE.IdNucleo + "','" +
												 UserE.Literal + "','" +
												 s.Name + "'," +
												 ((dominante++ == 0) ? true : false) + ")";

							cmd.ExecuteNonQuery();
						}
					}
				}
                else
                {
                    cmd.CommandText = "UPDATE Sectores SET PrioridadR2=" + UserE.R2Priority + " " +
                                                    "WHERE IdSector='" + UserE.Literal + "' AND " + "IdSistema='" + IdSistema + "' AND IdNucleo='" + Sectors[0].UserE.IdNucleo + "'";
                                                    
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('Sectores') ";
                    cmd.ExecuteNonQuery();

                    //if (UserE.Literal == sct.StrSectorFs)
                    //{
                    //    cmd.CommandText = "UPDATE SectoresSectorizacion SET IdTop='" + TopE + "'" +
                    //                        " WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + Sectors[0].UserE.IdSectorizacion + "' AND IdNucleo='" + Sectors[0].UserE.IdNucleo + 
                    //                        "' AND IdSector='" + UserE.Literal + "'";

                    //    cmd.ExecuteNonQuery();
                    //}
                }

			}
			catch (System.Data.SqlClient.SqlException )
			{	
				// Puede que el sector ya exista
			}

            if (UserE.Literal == sct.StrSectorFs)
            {
                // Eliminar la anterior asignación de sectores a Ucs
                cmd.CommandText = "DELETE FROM SectoresSectorizacion " +
                                    "WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + Sectors[0].UserE.IdSectorizacion +
                                    "' AND IdTop='" + TopE + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO SectoresSectorizacion (IdSistema,IdSectorizacion,IdNucleo,IdSector,IdTop)" +
                        " VALUES ('" + IdSistema + "','" +
                                     Sectors[0].UserE.IdSectorizacion + "','" +
                                     Sectors[0].UserE.IdNucleo + "','" +
                                     UserE.Literal + "','" +
                                     TopE + "')";

                cmd.ExecuteNonQuery();


                cmd.CommandText = "DELETE FROM SectoresSector " +
                                    "WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + Sectors[0].UserE.IdNucleo +
                                    "' AND IdSector='" + UserE.Literal + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO SectoresSector (IdSistema,IdNucleo,IdSector,IdSectorOriginal,EsDominante)" +
                        " VALUES ('" + IdSistema + "','" +
                                     Sectors[0].UserE.IdNucleo + "','" +
                                     UserE.Literal + "','" +
                                     UserE.Literal + "'," +
                                     true + ")";

                cmd.ExecuteNonQuery();

                cmd.CommandText = "REPLACE INTO TablasModificadas (IdTabla) VALUES ('SectoresSector') ";
                cmd.ExecuteNonQuery();
            }
            
            UserE.ATSNumbers.Sort(delegate(int x, int y) { return y - x; });  // De mayor a menor
            /*  PRUEBA PARA N'DJAMENA. 6/4/2015
             *  Como en ASECNA no hay PICTs con más de un sector, no es necesario
             *  componer los parámetros, números de abonados, teclas y permisos de red
             *  para la unión de sectores.
             *  Está ocurriendo con bastante asiduidad que después de manipular con los permisos de red
             *  y/o las redes, algunos sectores pierden los permisos, los números de abonado y las teclas.*/
            //if (Group == null)		// Sólo si no pertenece a una agrupación
            {
                // Generar parametrosSector de la combinación de sectores que componen esta ucs.
                UserE.GeneraParametrosSector(cmd);
                // Generar los números de abonados a los que atiende el sector resultante.
                UserE.GeneraNumerosAbonado(cmd);
            }

            UserE.GeneraTeclasSector(cmd);
            UserE.GeneraPermisosRedesSector(cmd);
            
			if (IsMultiOPE)
			{
				UserP.Literal = UserE.Literal + "." + extP;
				UserE.Literal += "." + extE;
				UserP.ATSNumbers.Sort(delegate(int x, int y) { return y - x; });
                //if (Group == null)		// Sólo si no pertenece a una agrupación
                {
                    // Generar parametrosSector de la combinación de sectores que componen esta ucs.
                    UserP.GeneraParametrosSector(cmd);
                    // Generar los números de abonados a los que atiende el sector resultante.
                    UserP.GeneraNumerosAbonado(cmd);
                }
            
                UserP.GeneraTeclasSector(cmd);
                UserP.GeneraPermisosRedesSector(cmd);
            }
		}

		//public void MergeUsers(Sectorization sct, DbCommand cmd)
		//{
		//    UserE = new UserInfo(Sectors[0].UserE);
		//    UserP = IsMultiOPE ? new UserInfo(Sectors[0].UserP) : null;

		//    UserE.CreaUser(Sectors[0].UserE);

		//    if (IsMultiOPE)
		//    {
		//        UserP.CreaUser(Sectors[0].UserP);
		//    }

		//    foreach (SectorInfo sector in Sectors)
		//    {
		//        UserE.Literal = sector.Name;

		//        UserE.MergeUser(sector.UserE);

		//        Debug.Assert((sector.UserP == null) || IsMultiOPE);
		//        if (IsMultiOPE)
		//        {
		//            UserP.Literal = sector.Name;

		//            UserP.MergeUser(sector.UserP);
		//        }
		//    }

		//    // Generar parametrosSector de la combinación de sectores que componen esta ucs.
		//    UserE.GeneraParametrosSector(cmd);
		//    // Generar los números de abonados a los que atiende el sector resultante.
		//    UserE.GeneraNumerosAbonado(cmd);

		//    if (IsMultiOPE)
		//    {
		//        // Generar parametrosSector de la combinación de sectores que componen esta ucs.
		//        UserP.GeneraParametrosSector(cmd);
		//        // Generar los números de abonados a los que atiende el sector resultante.
		//        UserP.GeneraNumerosAbonado(cmd);
		//    }
		//}



		public void MergeColaterals(Sectorization sct, DbCommand cmd, Tipo_Sectorizacion tipoSectorizacion)
		{
            //Se recorren todos los sectores configurados (uno o varios si se trata de una agrupación de sectores) en el puesto de operador que se está tratando
            //Para componer los colaterales del sector agrupado 

			foreach (SectorInfo sector in Sectors)
			{
				if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
					tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Telefonia)
				{
					UserE.MergeColateralsTf(sector.UserE, sct);
					UserE.MergeColateralsLc(sector.UserE, sct);
					UserE.MergeColateralsAg(sector.UserE, sct);
				}

				if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
					tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Radio)
					UserE.MergeColateralsRd(sector.UserE, sct); 

				//UserE.GeneraDestinosRadioSectorizados(cmd);
				/*
				// Genera los EstadosRecursos del usuario generado en la top. Tabla EstadoRecursos
				UserE.GeneraEstadoRecursos(cmd);
				// Genera los estados de altavoz del usuario generado en la top. Tabla EstadoAltavoces
				UserE.GeneraEstadoAltavoces(cmd);
				*/
				Debug.Assert((sector.UserP == null) || IsMultiOPE);
				if (IsMultiOPE)
				{
					if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
						tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Telefonia)
					{
						UserP.MergeColateralsTf(sector.UserP, sct);
						UserP.MergeColateralsLc(sector.UserP, sct);
						UserP.MergeColateralsAg(sector.UserP, sct);
					}

					if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
						tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Radio)
						UserP.MergeColateralsRd(sector.UserP, sct);
					//UserP.GeneraDestinosRadioSectorizados(cmd);
					/*
					// Genera los EstadosRecursos del usuario generado en la top. Tabla EstadoRecursos
					UserP.GeneraEstadoRecursos(cmd);
					// Genera los estados de altavoz del usuario generado en la top. Tabla EstadoAltavoces
					UserP.GeneraEstadoAltavoces(cmd);
					*/
				}
			}


			if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
				tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Telefonia)
			{
                //Se relacionan por el grupo los destinos ATS de la UCS que tienen asociados destinos de LC
                //Y se crea un idenficador único del grupo
                UserE.PanelLC_AsociaDestinosATSyLCEN();

				UserE.GeneraDestinosTelefoniaSectorizados(cmd);
				UserE.GeneraDestinosLCSectorizados(cmd);
				UserE.GeneraDestinosAgendaSectorizados(cmd);
			}

			if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
				tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Radio)
				UserE.GeneraDestinosRadioSectorizados(cmd);

			if (IsMultiOPE)
			{
				if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
					tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Telefonia)
				{
                    //Se relacionan por el grupo los destinos ATS de la UCS que tienen asociados destinos de LC
                    //Y se crea un idenficador único del grupo
                    UserE.PanelLC_AsociaDestinosATSyLCEN();

					UserP.GeneraDestinosTelefoniaSectorizados(cmd);
					UserP.GeneraDestinosLCSectorizados(cmd);
					UserP.GeneraDestinosAgendaSectorizados(cmd);
				}
				
				if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa ||
					tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Radio)
					UserP.GeneraDestinosRadioSectorizados(cmd);
			}
		}
   }

    /// <summary>
    /// 
    /// </summary>
	public class Sectorization
	{
        private string STR_SECTOR_FS = "**FS**";
        private const int INT_SECTOR_FS = 20000;

		public string Name;
		private string IdSistema;
		private bool HacerComprobacionTodosSectoresReales;

		private StringBuilder StringSectores = new StringBuilder();

		// Ucs, sectores, usuarios, ATSNumbers y PublicNumbers involucrados en la sectorización
		internal Dictionary<int, UcsInfo> ListOfUcs = new Dictionary<int, UcsInfo>();
		internal Dictionary<int, SectorInfo> ListOfSectors = new Dictionary<int, SectorInfo>();
		internal List<int> ListOfDominantUsers = new List<int>();
		internal Dictionary<string, UserInfo> ListOfUsersByName = new Dictionary<string, UserInfo>();
		internal Dictionary<string, int> ListOfDestinations = new Dictionary<string, int>();

		// Nombre de todas las agrupaciones definidas
		internal Dictionary<string, SectorGroupInfo> ListOfGroups = new Dictionary<string, SectorGroupInfo>();

		// Nombre + ConfigId de todos los usuarios definidos
		public List<string> ListOfAllUserNames = new List<string>();

		// Fecha y hora de creación de la sectorización. Sólo se usa en las sectorizaciones desde SACTA
		DateTime FechaActivacion = new DateTime();

        Dictionary<string, int> _LceGroupsResourcesCount = new Dictionary<string, int>();
        public Dictionary<string, int> LceGroupsResourcesCount
        {
            get { return _LceGroupsResourcesCount; }
        }


        public char CaracterEjecutivo;
        public char CaracterPlanificador;

        public string StrSectorFs 
        {
            get { return STR_SECTOR_FS; }
        }

		public void EliminaInternosSectorizados(DbCommand cmd)
		{
			// Eliminar los internos para este sector
			cmd.CommandText = "DELETE FROM Internos WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + Name + "'";
			cmd.ExecuteNonQuery();
		}

		public void EliminaExternosSectorizados(DbCommand cmd)
		{
			// Eliminar los internos para este sector
			cmd.CommandText = "DELETE FROM Externos WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + Name + "'";
			cmd.ExecuteNonQuery();
		}

		public void EliminaRadioSectorizados(DbCommand cmd)
		{
			// Eliminar los internos para este sector
			cmd.CommandText = "DELETE FROM Radio WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='" + Name + "'";
			cmd.ExecuteNonQuery();
		}

		public Sectorization(DbCommand cmd, string idSistema, string name, string data, char extE, char extP, Tipo_Sectorizacion tipoSectorizacion, DateTime fechaActivacion, bool comprobarTodosLosReales)
		{
            string strModulo = "Sectoritation.Sectorizacion";
			Name = name;
			IdSistema = idSistema;
			CaracterEjecutivo = extE;
			CaracterPlanificador = extP;
			FechaActivacion = fechaActivacion;
			HacerComprobacionTodosSectoresReales = comprobarTodosLosReales;

            Log(false, strModulo, "Entrando Data={0} Sectorization name={1} ...", data, Name);
            string[] sectorUcs = data.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
			Debug.Assert(sectorUcs.Length % 2 == 0);

			if (sectorUcs.Length == 0)
			{
				throw new SectorizationException(SectorizationResult.EmptySectorization, Resources.EmptySectError);
			}

			#region Parseo de datos de entrada y obtencion de ucs y sectores

            Log(false, strModulo, "Parseo de datos de entrada y obtencion de ucs y sectores...");
            for (int i = 0, total = sectorUcs.Length; i < total; i += 2)
			{
				int ucsId = Int32.Parse(sectorUcs[i + 1]);
				UcsInfo ucs = null;

				if (!ListOfUcs.TryGetValue(ucsId, out ucs))
				{
					ucs = new UcsInfo(idSistema);
					ListOfUcs[ucsId] = ucs;
                    ListOfDominantUsers.Add(Int32.Parse(sectorUcs[i]));
				}

				int sectorId = Int32.Parse(sectorUcs[i]);
				SectorInfo sector = new SectorInfo(ucs, sectorId);

                Debug.Assert(!ListOfSectors.ContainsKey(sectorId));
                ListOfSectors[sectorId] = sector;

				ucs.Sectors.Add(sector);

				// Componer lista de idSector para consultas
				StringSectores.AppendFormat("{0}{1}", sectorId, ",");
			}

            // Crear UCS virtual y sector para posiciones fuera de sectorización
            //UcsInfo ucsFS = new UcsInfo(idSistema);
            //ListOfUcs[0] = ucsFS;
            //SectorInfo sectorFS = new SectorInfo(ucsFS, INT_SECTOR_FS);
            //ListOfSectors[INT_SECTOR_FS] = sectorFS;
            //ucsFS.Sectors.Add(sectorFS);
			#endregion

			// Eliminar última coma de StringSectores
			StringSectores.Remove(StringSectores.Length - 1, 1);

			#region Obtención de información desde la base de datos
            
            Log(false, strModulo, "Obtención de información desde la base de datos StringSectores={0}...", StringSectores);

            //Se obtiene la lista de agrupaciones y la lista de sectores que componen cada agrupación
            GetGroupsInfo(cmd);
			GetUcsInfo(cmd);
			GetSectorsInfo(cmd);

            // Recoger informacion de los grupos de LC y obtener el número de destinos que componen cada grupo en _LceGroupsResourcesCount
            GetInfoLCGroups(cmd);

			// Eliminar los sectores de mantto. mezclados con los sectores reales o virtuales. 
			// Se supone que esto nunca va a pasar
			if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa)
				RemoveMantSectors(); 

            //De todos los sectores simples que forman parte de la sectorización, se recupera de cada sector:
            //los parámetros, sus destinos externos e internos de telefonía, la agenda y los destinos radio
            //Además, en sector FS, se añaden todos los destinos radios configurados en los distintos sectores.
			GetUsersInfo(cmd);
			#endregion

			#region Eliminar sectorización anterior
            
            Log(false, strModulo, "Eliminar sectorización anterior...");
            if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa || tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Telefonia)
			{
				EliminaExternosSectorizados(cmd);
				EliminaInternosSectorizados(cmd);
			}
			if (tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Completa || tipoSectorizacion == Tipo_Sectorizacion.Sectorizacion_Radio)
				EliminaRadioSectorizados(cmd);
			#endregion

            if (Name == "SACTA")
            {
                // Generar registro SACTA en tabla Sectorizaciones
                Log(false, strModulo, "Generar registro SACTA en tabla Sectorizaciones...");
                CreaSactaEnSectorizaciones(cmd);
            }

            Log(false, strModulo, "foreach (UcsInfo ucs in ListOfUcs.Values) ucs.MergeUsers...");
			foreach (UcsInfo ucs in ListOfUcs.Values)
			{
                ucs.MergeUsers(this, cmd, extE, extP);
                
                //CreaSectores(cmd, ucs);
                if (Name == "SACTA")
                {
                    Log(false, strModulo, "CreaSectores(cmd, ucs ({0}) )...", ucs.TopE);
                    CreaSactaEnSectoresSectorizaciones(cmd, ucs);
#if DEBUG1
                    System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
#endif
                }

            }

            //Para cada puesto de operador (ucs) de la sectorización, se generan los colaterales
            Log(false, strModulo, "foreach (UcsInfo ucs in ListOfUcs.Values) ucs.MergeColaterals...");
            foreach (UcsInfo ucs in ListOfUcs.Values)
			{
                Log(false, strModulo, "ucs.MergeColaterals ucs= " + ucs.TopE);
				ucs.MergeColaterals(this, cmd, tipoSectorizacion);
#if DEBUG1
                System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5)).Wait();
#endif
            }

            Log(false, strModulo, "Saliendo...");
        }

        /*
		private void CreaSectores(DbCommand cmd, UcsInfo ucs)
		{
			try
			{
				bool insertar = false;
				cmd.CommandText = "SELECT * FROM Sectores WHERE IdSistema='" + IdSistema + "' AND IdNucleo='" + ucs.Sectors[0].UserE.IdNucleo +
									"' AND IdSector='" + ucs.UserE.Literal + "'";
				using (DbDataReader dr = cmd.ExecuteReader())
				{
					insertar = !dr.HasRows;
				}

				if (insertar)
				{
                    cmd.CommandText = "INSERT INTO Sectores (IdSistema,IdNucleo,IdSector,SectorSimple,Tipo,TipoPosicion,PrioridadR2," +
                                                            "NumSacta)" +
                                    " VALUES ('" + IdSistema + "','" +
                                                 ucs.Sectors[0].UserE.IdNucleo + "','" +
                                                 ucs.UserE.Literal + "'," +
                                                 false + ",'" +
                                                 ucs.Sectors[0].UserE.Sector.Type + "','" +
                                                 CaracterEjecutivo.ToString() + "'," +
                                                 ucs.UserE.R2Priority + "," +
                                                 ucs.Sectors[0].UserE.Sector.IdSector + ")";
                    //cmd.CommandText = "INSERT INTO Sectores (IdSistema,IdNucleo,IdSector,SectorSimple)" +
                    //                    " VALUES ('" + IdSistema + "','" +
                    //                                 ucs.Sectors[0].UserE.IdNucleo + "','" +
                    //                                 ucs.UserE.Literal + "'," +
                    //                                 false + ")";
					cmd.ExecuteNonQuery();

					// Crear SectoresSector
					if (Name == "SACTA")
					{
						int dominante = 0;
						foreach (SectorInfo s in ucs.Sectors)
						{
							cmd.CommandText = "INSERT INTO SectoresSector (IdSistema,IdNucleo,IdSector,IdSectorOriginal,EsDominante)" +
									" VALUES ('" + IdSistema + "','" +
												 s.UserE.IdNucleo + "','" +
												 ucs.UserE.Literal + "','" +
												 s.Name + "'," +
												 ((dominante++ == 0) ? true : false) + ")";

							cmd.ExecuteNonQuery();
						}
					}
				}
                else
                {
                    cmd.CommandText = "UPDATE Sectores SET PrioridadR2=" + ucs.UserE.R2Priority + " " +
                                                    "WHERE IdSector='" + ucs.UserE.Literal + "' AND " + "IdSistema='" + IdSistema + "' AND IdNucleo='" + ucs.Sectors[0].UserE.IdNucleo + "'";
                                                    
                    cmd.ExecuteNonQuery();
                }

			}
			catch (System.Data.SqlClient.SqlException e)
			{	
				// Puede que el sector ya exista
			}
		}
        */

		private void CreaSactaEnSectoresSectorizaciones(DbCommand cmd, UcsInfo ucs)
		{
			string nomAgrupacion = ucs.UserE.Literal;
			string idNucleo = ucs.Sectors[0].UserE.IdNucleo;
			string idTop = ucs.TopE;

			cmd.CommandText = "REPLACE INTO SectoresSectorizacion (IdSistema,IdSectorizacion,IdNucleo,IdSector,IdTop)" +
								" VALUES ('" + IdSistema + "','" +
											 "SACTA" + "','" +
											 idNucleo + "','" +
											 nomAgrupacion + "','" +
											 idTop + "')";
			cmd.ExecuteNonQuery();
		}

		private void CreaSactaEnSectorizaciones(DbCommand cmd)
		{
			bool insertar = false;
			// Crear registro con IdSectorizacion='SACTA' si es que no existe
			cmd.CommandText = "SELECT * FROM Sectorizaciones " +
								"WHERE IdSistema='" + IdSistema + "' AND " +
										"IdSectorizacion='SACTA'";
			using (DbDataReader dr = cmd.ExecuteReader())
			{
				insertar = !dr.HasRows;
			}

			if (insertar)
			{

				cmd.CommandText = "INSERT INTO Sectorizaciones (IdSistema,IdSectorizacion,Activa,FechaActivacion)" +
								" VALUES ('" + IdSistema + "','" +
											 "SACTA" + "'," +
											 false + "," +
											 "STR_TO_DATE('" + FechaActivacion.ToString("dd/MM/yyyy HH:mm:ss") + "','%d/%m/%Y %H:%i:%s')" +
											   " )";
				cmd.ExecuteNonQuery();
			}

			// Eliminar la anterior asignación de sectores a Ucs
			cmd.CommandText = "DELETE FROM SectoresSectorizacion " +
								"WHERE IdSistema='" + IdSistema + "' AND IdSectorizacion='SACTA'";
			cmd.ExecuteNonQuery();
		}

		#region Private

        void GetInfoLCGroups(DbCommand cmd)
        {
            cmd.CommandText = "SELECT dt.IdGrupo,COUNT(*) FROM destinostelefonia dt " +
	                                "INNER JOIN grupostelefonia gt ON gt.idgrupo=dt.idgrupo " +
	                                "where dt.idgrupo IS NOT NULL GROUP BY dt.idgrupo";
            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    _LceGroupsResourcesCount[dr.GetString(0)] = dr.GetInt32(1);
                }
            }

        }

		void GetGroupsInfo(DbCommand cmd)
		{
			#region Obtenemos las agrupaciones
			cmd.CommandText = "SELECT sa.IdAgrupacion, s.NumSacta FROM SectoresAgrupacion sa, sectores s " +
								"WHERE sa.IdSistema='" + IdSistema + "' AND " +
										"s.IdSistema=sa.IdSistema AND " +
										"sa.IdSector=s.IdSector " +
								"ORDER BY sa.IdAgrupacion";
			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					string groupId = dr.GetString(0);
					SectorGroupInfo group = null;

					if (!ListOfGroups.TryGetValue(groupId, out group))
					{
						group = new SectorGroupInfo();
						group.Name = groupId;
						ListOfGroups[groupId] = group;
						//group.Especial = dr.IsDBNull(2) || (dr.GetInt32(2) == 0) ? false : true;
					}

					group.Sectors.Add(dr.GetInt32(1));
				}
			}
			#endregion

			foreach (SectorGroupInfo group in ListOfGroups.Values)
			{
				group.Sectors.Sort();

				if (group.Especial)
				{
					#region Obtenemos los colaterales asociados al grupo especial
					//Dictionary<string, string> publicNumbers = new Dictionary<string, string>();
					//group.TlfEndPointsE = new TLFEndPoint[67];
					//group.TlfEndPointsP = new TLFEndPoint[67];

					//// Los destinos ATS ademas de número R2 pueden tener número RTB (Público = 4)
					//cmd.CommandText = "SELECT Destino, Numero FROM Destinos WHERE PrefijoRed = 4";
					//using (DbDataReader dr = cmd.ExecuteReader())
					//{
					//    while (dr.Read())
					//    {
					//        string dst = dr.GetString(0);

					//        Debug.Assert(!publicNumbers.ContainsKey(dst));
					//        publicNumbers[dst] = dr.GetString(1);
					//    }
					//}

					//cmd.CommandText = "SELECT COUNT(*) FROM DestinosLargos";
					//object objCount = cmd.ExecuteScalar();
					//if ((objCount != null) && (Int32.Parse(objCount.ToString()) > 0))
					//{
					//    cmd.CommandText = "SELECT DISTINCT A.Origen, A.Ring, A.Prioridad, B.Destino, B.Numero, B.PrefijoRed, D.PosFinal, D.TipoPosicion " +
					//                        "FROM ColateralTF A, Destinos B, DestinosLargos C, PanelAgrupacion D " +
					//                        "WHERE D.NomAgrupacion = '" + group.Name + "' AND D.NumUsuario = 55 AND A.IdConfig = D.IdConfig " +
					//                        "AND A.NumUsuario = D.NumUsuario AND A.IdPanel = D.IdPanel AND A.Posicion = D.Posicion " +
					//                        "AND ((A.Destino = B.Destino) OR (RTRIM(A.Destino) = C.DestinoLargo AND C.DestinoCorto = RTRIM(B.Destino) And C.Tipo = B.PrefijoRed)) " +
					//                        "AND ((A.PrefijoRed = B.PrefijoRed) OR (A.PrefijoRed = 2 AND B.PrefijoRed = 7)) ORDER BY A.IdConfig, A.NumUsuario, A.Posicion";
					//}
					//else
					//{
					//    cmd.CommandText = "SELECT DISTINCT A.Origen, A.Ring, A.Prioridad, B.Destino, B.Numero, B.PrefijoRed, D.PosFinal, D.TipoPosicion " +
					//                            "FROM ColateralTF A, Destinos B, PanelAgrupacion D " +
					//                            "WHERE D.NomAgrupacion = '" + group.Name + "' AND D.NumUsuario = 55 AND A.IdConfig = D.IdConfig " +
					//                            "AND A.NumUsuario = D.NumUsuario AND A.IdPanel = D.IdPanel AND A.Posicion = D.Posicion " +
					//                            "AND A.Destino = B.Destino AND ((A.PrefijoRed = B.PrefijoRed) OR (A.PrefijoRed = 2 AND B.PrefijoRed = 7)) " +
					//                                "ORDER BY A.IdConfig, A.NumUsuario, A.Posicion";
					//}

					//using (DbDataReader dr = cmd.ExecuteReader())
					//{
					//    while (dr.Read())
					//    {
					//        TLFEndPoint ep = new TLFEndPoint();
					//        string type = dr.GetString(7);

					//        ep.Group = dr.GetInt32(0);
					//        ep.Ring = dr.GetInt32(1);
					//        ep.Priority = dr.GetInt32(2);
					//        ep.Name = dr.GetString(3);
					//        ep.Number = dr.GetString(4);
					//        ep.Prefix = (PrefixType)dr.GetInt32(5);
					//        ep.Position = dr.GetInt32(6);

					//        if (type == "E")
					//        {
					//            if (ep.Position >= group.TlfEndPointsE.Length)
					//                {
					//                Array.Resize(ref group.TlfEndPointsE, ep.Position + 1);
					//                }
					//            group.TlfEndPointsE[ep.Position] = ep;
					//        }
					//        else
					//        {
					//            if (ep.Position >= group.TlfEndPointsP.Length)
					//            {
					//                Array.Resize(ref group.TlfEndPointsP, ep.Position + 1);
					//            }
					//            group.TlfEndPointsP[ep.Position] = ep;
					//        }

					//        if (ep.Prefix == PrefixType.ATS)
					//        {
					//            publicNumbers.TryGetValue(ep.Name, out ep.PublicNumber);
					//        }
					//    }
					//}
					#endregion
				}
			}
		}

		void GetUcsInfo(DbCommand cmd)
		{
			cmd.CommandText = "SELECT IdTop,PosicionSacta FROM Top WHERE IdSistema='" + IdSistema + "'";
			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					int ucsId = dr.GetInt32(1);
					UcsInfo ucs = null;

					if (ListOfUcs.TryGetValue(ucsId, out ucs))
					{
						// De momento todas las sectorizaciones son con UCS monousuario
						ucs.TopE = dr.GetString(0);
						//string top = dr.GetString(0);
						//if (dr.GetString(2) == "E")
						//{
						//    ucs.TopE = top;
						//}
						//else
						//{
						//    ucs.TopP = top;
						//}
	               }
                    else
                    {
                        UcsInfo ucsFS = new UcsInfo(IdSistema);
                        ucsFS.TopE = dr.GetString(0);
                        ListOfUcs[ucsId] = ucsFS;
                        //SectorInfo sectorFS = new SectorInfo(ucsFS, INT_SECTOR_FS);
                        //ListOfSectors[INT_SECTOR_FS] = sectorFS;
                        //ucsFS.Sectors.Add(sectorFS);
                    }
				}
			}
		}

		void GetSectorsInfo(DbCommand cmd)
		{
            string idNucleo = string.Empty;

            cmd.CommandText = "SELECT DISTINCT(IdNucleo) FROM SectoresSectorizacion WHERE IdSistema='" + IdSistema + 
                                "' AND IdSectorizacion='" + Name +"'";
            using (DbDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    idNucleo = dr.GetString(0);
                }
            }

            //cmd.CommandText = "SELECT ss.IdNucleo,ss.IdSectorOriginal,ss.IdSector,sc.tipo,sc.Numsacta,sc.TipoPosicion,sc.PrioridadR2 " +
            //                    "FROM sectoressector ss, sectores sc WHERE	sc.IdSistema='" + IdSistema + "' AND " +
            //                        "sc.SectorSimple AND " +
            //                        "sc.IdSistema=ss.IdSistema AND " +
            //                        "sc.IdNucleo=ss.IdNucleo AND " +
            //                        "sc.IdSector=ss.IdSectorOriginal AND " +
            //                        "sc.IdSector=ss.IdSector";
            cmd.CommandText = "SELECT IdNucleo,IdSector,tipo,Numsacta,TipoPosicion,PrioridadR2 " +
                                "FROM sectores WHERE IdSistema='" + IdSistema + "' AND " +
                                    "SectorSimple";

			using (DbDataReader dr = cmd.ExecuteReader())
			{
				Dictionary<string, int> realSectorsByNucleo = new Dictionary<string, int>();
				Dictionary<string, int> realSectorsByNucleoInSect = new Dictionary<string, int>();

				while (dr.Read())
				{
                    //if (idNucleo != string.Empty && dr.GetString(0) != idNucleo)
                    //    continue;

					int sectorId = dr.GetInt32(3);
					string type = dr.GetString(2);
					SectorInfo sector = null;

                    if (ListOfSectors.TryGetValue(sectorId, out sector))
                    {
                        idNucleo = dr.GetString(0);
                        if (type == "R")
                        {	// Número de sectores reales por núcleo
                            int numRealSectorsByNucleo = 0;

                            realSectorsByNucleo.TryGetValue(idNucleo, out numRealSectorsByNucleo);
                            realSectorsByNucleo[idNucleo] = ++numRealSectorsByNucleo;
                        }

                        if (sector.IdNucleo == null)
                        {
                            sector.IdNucleo = idNucleo;
                            sector.Name = dr.GetString(1);
                            sector.Type = type;
                        }
                        sector.IdSistema = IdSistema;

                        int numRSectors = 0;
                        int numToAdd = sector.Type == "R" ? 1 : 0;

                        realSectorsByNucleoInSect.TryGetValue(idNucleo, out numRSectors);
                        realSectorsByNucleoInSect[idNucleo] = numRSectors + numToAdd;

                        string userId = dr.GetString(1);

                        //PENDIENTE DE REVISAR
                        //OJO!!!!!: cuando el TipoPosicion== Planificador (<>Usuario Controlador), la sectorización falla al provocarse una excepción
                        // porque en algunas partes del código se está accediendo a la variable sector.UserE 
                        // cuando en este caso la variable tiene valor null.
                        //En la versión actual para torre, se ha modificado la aplicación WEB de CONFIGURACIÓN PARA QUE EL TIPO DE POSICIÓN TOME SIEMPRE EL VALOR "C" --> Controlador

                        if (dr.GetString(4)[0] == CaracterEjecutivo)	// Usuario Controlador
                        {
                            UserInfo user = new UserInfo(sector, sector.Ucs.TopE, userId);
                            user.IdNucleo = idNucleo;
                            user.IdSistema = IdSistema;
                            user.IdSectorizacion = Name;
                            user.Name = userId;
                            user.R2Priority = (int)((uint)dr["PrioridadR2"]);
                            ListOfUsersByName[userId] = user;
                            sector.UserE = user;
                        }
                        else      // Usuario Planificador
                        {
                            UserInfo user = new UserInfo(sector, sector.Ucs.TopP, userId);
                            user.IdNucleo = idNucleo;
                            user.IdSistema = IdSistema;
                            user.IdSectorizacion = Name;
                            user.Name = userId;
                            user.R2Priority = (int)((uint)dr["PrioridadR2"]);
                            ListOfUsersByName[userId] = user;
                            sector.UserP = user;
                        }

                        ListOfAllUserNames.Add(userId);
                    }
                    else
                    {
                        if (type == "R")
                        {
                            int numRealSectorsByNucleo = 0;

                            realSectorsByNucleo.TryGetValue(dr.GetString(0), out numRealSectorsByNucleo);
                            realSectorsByNucleo[dr.GetString(0)] = ++numRealSectorsByNucleo;
                        }
                    }
				}

                // Añadir el usuario "Fuera de sectorización"
                AnadeUsuarioFueraSectorizacion(idNucleo);

				if (HacerComprobacionTodosSectoresReales)
				{
					foreach (KeyValuePair<string, int> keyValue in realSectorsByNucleoInSect)
					{
						int numRSectorsByNucleo = 0;
						realSectorsByNucleo.TryGetValue(keyValue.Key, out numRSectorsByNucleo);

						if (numRSectorsByNucleo > keyValue.Value)
						{
                            Log(true, "Sectoritation.GetSectorsInfo", "No Coinciden los Sectores Reales. {0}:{1}", numRSectorsByNucleo, keyValue.Value);
                            throw new SectorizationException(SectorizationResult.MissingRealSectors, Resources.MissingRealSectorError);
						}
					}
				}
			}
		}

        private void AnadeUsuarioFueraSectorizacion(string idNucleo)
        {
            foreach (KeyValuePair<int,UcsInfo> ucsFS in ListOfUcs)
            {
                if (ucsFS.Value.Sectors.Count == 0 || ucsFS.Value.Sectors[0].IdSector == INT_SECTOR_FS)
                {
                    int sectorID = INT_SECTOR_FS;
                    SectorInfo sector;

                    if (ucsFS.Value.Sectors.Count == 0)
                    {
                        sector = new SectorInfo(ucsFS.Value, sectorID);
                        ListOfSectors[sectorID] = sector;
                        ucsFS.Value.Sectors.Add(sector);
                    }
                    else
                    {
                        sector = ListOfSectors[sectorID];
                    }

                    UserInfo user = new UserInfo(sector, sector.Ucs.TopE, STR_SECTOR_FS);
                    user.IdNucleo = idNucleo;
                    user.IdSistema = IdSistema;
                    user.IdSectorizacion = Name;
                    user.Name = STR_SECTOR_FS;
                    user.R2Priority = 3;
                    user.RdEndPoints = new RDEndPoint[135];
                    ListOfUsersByName[STR_SECTOR_FS] = user;

                    sector.IdNucleo = idNucleo;
                    sector.Name = STR_SECTOR_FS;
					sector.Type = "R";
					sector.IdSistema = IdSistema;

                    sector.UserE = user;
                }
            }
        }

		void GetUsersInfo(DbCommand cmd)
		{
            // Sector FS
            SectorInfo usrFS = null;
            ListOfSectors.TryGetValue(INT_SECTOR_FS, out usrFS);

			#region Obtención de información básica

            cmd.CommandText = "SELECT ts.idsector, ps.*, " +
                                    "ts.TransConConsultaPrev,ts.TransDirecta,ts.Conferencia,ts.Escucha,ts.Retener,ts.Captura,ts.Redireccion,ts.RepeticionUltLlamada,ts.RellamadaAut," +
                                    "ts.TeclaPrioridad,ts.Tecla55mas1,ts.Monitoring,ts.CoordinadorTF,ts.CoordinadorRD,ts.IntegracionRDTF,ts.LlamadaSelectiva,ts.GrupoBSS,ts.LTT,ts.SayAgain,ts.InhabilitacionRedirec," +
                                    "ts.Glp FROM teclassector ts, sectores s, parametrossector ps " +
                                    "WHERE s.IdSistema='" + IdSistema + "' AND " +
                                        "s.SectorSimple AND	s.NumSacta IN (" + StringSectores.ToString() + ") AND " +
                                        "ps.IdSistema=s.IdSistema AND ps.IdNucleo=s.IdNucleo AND ps.IdSector=s.IdSector AND " +
                                        "ts.IdSistema=s.IdSistema AND ts.IdNucleo=s.IdNucleo AND ts.IdSector=s.IdSector";

			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					string userName = (string)dr["IdSector"];
					UserInfo user = null;

					//ListOfAllUserNames.Add(userName);

					if (ListOfUsersByName.TryGetValue(userName, out user))
					{
						if (dr["NumLlamadasEntrantesIDA"] != System.DBNull.Value)
							user.NumLLamadasEntrantesIDA = (uint)dr["NumLlamadasEntrantesIDA"];
						if (dr["NumLlamadasEnIDA"] != System.DBNull.Value)
							user.NumLlamadasEnIDA = (uint)dr["NumLlamadasEnIDA"];
						if (dr["NumFreqPagina"] != System.DBNull.Value)
							user.NumFreqPagina = (uint)dr["NumFreqPagina"];
						if (dr["NumPagFreq"] != System.DBNull.Value)
							user.NumPagFreq = (uint)dr["NumPagFreq"];
						if (dr["NumDestinosInternosPag"] != System.DBNull.Value)
							user.NumDestinosInternosPag = (uint)dr["NumDestinosInternosPag"];
						if (dr["NumPagDestinosInt"] != System.DBNull.Value)
							user.NumPagDestinosInt = (uint)dr["NumPagDestinosInt"];
						if (dr["Intrusion"] != System.DBNull.Value)
							user.Intrusion = (bool)dr["Intrusion"];
						if (dr["Intruido"] != System.DBNull.Value)
							user.Intruido = (bool)dr["Intruido"];
						if (dr["KeepAlivePeriod"] != System.DBNull.Value)
							user.KAP = (uint)dr["KeepAlivePeriod"];
                        if (dr["KeepAliveMultiplier"] != System.DBNull.Value)
                            user.KAM = (uint)dr["KeepAliveMultiplier"];
                        if (dr["NumEnlacesAI"] != System.DBNull.Value)
                            user.NumEnlacesAI = (uint)dr["NumEnlacesAI"];

                        if (dr["GrabacionEd137"] != System.DBNull.Value)
                            user.GrabacionEd137 = (bool)dr["GrabacionEd137"];


                        if (dr["TransConConsultaPrev"] != System.DBNull.Value)
                            user.TransConConsultaPrev = (bool)dr["TransConConsultaPrev"];
                        if (dr["TransDirecta"] != System.DBNull.Value)
                            user.TransDirecta = (bool)dr["TransDirecta"];
                        if (dr["Conferencia"] != System.DBNull.Value)
                            user.Conferencia = (bool)dr["Conferencia"];
                        if (dr["Escucha"] != System.DBNull.Value)
                            user.Escucha = (bool)dr["Escucha"];
                        if (dr["Retener"] != System.DBNull.Value)
                            user.Retener = (bool)dr["Retener"];
                        if (dr["Captura"] != System.DBNull.Value)
                            user.Captura = (bool)dr["Captura"];
                        if (dr["Redireccion"] != System.DBNull.Value)
                            user.Redireccion = (bool)dr["Redireccion"];
                        if (dr["RepeticionUltLlamada"] != System.DBNull.Value)
                            user.RepeticionUltLlamada = (bool)dr["RepeticionUltLlamada"];
                        if (dr["RellamadaAut"] != System.DBNull.Value)
                            user.RellamadaAut = (bool)dr["RellamadaAut"];
                        if (dr["TeclaPrioridad"] != System.DBNull.Value)
                            user.TeclaPrioridad = (bool)dr["TeclaPrioridad"];
                        if (dr["Tecla55mas1"] != System.DBNull.Value)
                            user.Tecla55mas1 = (bool)dr["Tecla55mas1"];
                        if (dr["Monitoring"] != System.DBNull.Value)
                            user.Monitoring = (bool)dr["Monitoring"];
                        if (dr["CoordinadorTF"] != System.DBNull.Value)
                            user.CoordinadorTF = (bool)dr["CoordinadorTF"];
                        if (dr["CoordinadorRD"] != System.DBNull.Value)
                            user.CoordinadorRD = (bool)dr["CoordinadorRD"];
                        if (dr["IntegracionRDTF"] != System.DBNull.Value)
                            user.IntegracionRDTF = (bool)dr["IntegracionRDTF"];
                        if (dr["LlamadaSelectiva"] != System.DBNull.Value)
                            user.LlamadaSelectiva = (bool)dr["LlamadaSelectiva"];
                        if (dr["GrupoBSS"] != System.DBNull.Value)
                            user.GrupoBSS = (bool)dr["GrupoBSS"];
                        if (dr["LTT"] != System.DBNull.Value)
                            user.LTT = (bool)dr["LTT"];
                        if (dr["SayAgain"] != System.DBNull.Value)
                            user.SayAgain = (bool)dr["SayAgain"];
                        if (dr["InhabilitacionRedirec"] != System.DBNull.Value)
                            user.InhabilitacionRedirec = (bool)dr["InhabilitacionRedirec"];
                        if (dr["Glp"] != System.DBNull.Value)
                            user.Glp = (bool)dr["Glp"];

						user.TlfEndPoints = new TLFEndPoint[user.NumDestinosInternosPag * user.NumPagDestinosInt];
						user.LCEndPoints = new TLFEndPoint[user.NumDestinosInternosPag * user.NumPagDestinosInt];
						user.AgEndPoints = new TLFEndPoint[user.NumDestinosInternosPag * user.NumPagDestinosInt];
						user.RdEndPoints = new RDEndPoint[user.NumFreqPagina * user.NumPagFreq];

                        if (usrFS != null)
                        {
                            usrFS.UserE.NumDestinosInternosPag = user.NumDestinosInternosPag > usrFS.UserE.NumDestinosInternosPag ? user.NumDestinosInternosPag : usrFS.UserE.NumDestinosInternosPag;
                            usrFS.UserE.NumFreqPagina = user.NumFreqPagina > usrFS.UserE.NumFreqPagina ? user.NumFreqPagina : usrFS.UserE.NumFreqPagina;
                            usrFS.UserE.NumLlamadasEnIDA = user.NumLlamadasEnIDA > usrFS.UserE.NumLlamadasEnIDA ? user.NumLlamadasEnIDA : usrFS.UserE.NumLlamadasEnIDA;
                            usrFS.UserE.NumLLamadasEntrantesIDA = user.NumLLamadasEntrantesIDA > usrFS.UserE.NumLLamadasEntrantesIDA ? user.NumLLamadasEntrantesIDA : usrFS.UserE.NumLLamadasEntrantesIDA;
                            usrFS.UserE.NumPagDestinosInt = user.NumPagDestinosInt > usrFS.UserE.NumPagDestinosInt ? user.NumPagDestinosInt : usrFS.UserE.NumPagDestinosInt;
                            usrFS.UserE.NumPagFreq = user.NumPagFreq > usrFS.UserE.NumPagFreq ? user.NumPagFreq : usrFS.UserE.NumPagFreq;
                            usrFS.UserE.NumEnlacesAI = user.NumEnlacesAI > usrFS.UserE.NumEnlacesAI ? user.NumEnlacesAI : usrFS.UserE.NumEnlacesAI;
                        }
					}
				}

				//ListOfAllUserNames.Sort();
			}
         #endregion


            #region Obtención permisos sobre las redes
            cmd.CommandText = "SELECT pr.IdSector, r.Idred, pr.Llamar, pr.Recibir FROM redes r LEFT OUTER JOIN permisosredes pr ON " +
                            "r.IdSistema = pr.IdSistema AND r.IdRed = pr.IdRed AND r.IdSistema ='" + IdSistema + "' WHERE r.IdPrefijo<>3";

            using (DbDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    if (dr["IdSector"] != System.DBNull.Value)
                    {
                        string userName = (string)dr["IdSector"];
                        UserInfo user = null;

                        //ListOfAllUserNames.Add(userName);

                        if (ListOfUsersByName.TryGetValue(userName, out user))
                        {
                            if (dr["IdRed"] != System.DBNull.Value)
                            {
                                PermisoRed pr;

                                if (user.PermisoRedes.TryGetValue((string)dr["IdRed"], out pr))
                                {
                                    if (dr["Llamar"] != System.DBNull.Value)
                                        pr.Llamar = (string)dr["Llamar"] != "false";
                                    if (dr["Recibir"] != System.DBNull.Value)
                                        pr.Recibir = (string)dr["Recibir"] != "false";

                                    user.Sector.UserE.PermisoRedes[(string)dr["IdRed"]] = pr;
                                }
                                else
                                {
                                    if (dr["Llamar"] != System.DBNull.Value)
                                        pr.Llamar = (string)dr["Llamar"] != "false";
                                    if (dr["Recibir"] != System.DBNull.Value)
                                        pr.Recibir = (string)dr["Recibir"] != "false";

                                    //MVO: se protege el acceso, para el sector no controlador UserE es nulo
                                    if (null != user.Sector.UserE)
                                    {
                                        user.Sector.UserE.PermisoRedes.Add((string)dr["IdRed"], pr);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            ListOfAllUserNames.Sort();

			//GetUsersPublicNumbers(cmd);
			//GetUsersAtsNumbers(cmd);
			GetUsersNumbers(cmd);
			GetUsersTlfColaterals(cmd);
			GetUsersTlfAgenda(cmd);
			GetUsersRdColaterals(cmd);
		}

		void GetUsersNumbers(DbCommand cmd)
		{
			//cmd.CommandText = "SELECT ua.IdSector,ua.IdPrefijo,ua.IdAbonado FROM SectoresSectorizacion sz, SectoresSector ss, UsuariosAbonados ua " +
			//                    "WHERE sz.IdSistema='" + IdSistema + "' AND sz.IdSectorizacion='" + Name + "' AND ss.IdSector=sz.IdSector AND " +
			//                    "ua.IdSistema=ss.IdSistema AND ua.IdNucleo=ss.IdNucleo AND ua.IdSector=ss.IdSectorOriginal " +
			//                    "AND ua.IdPrefijo<>3";
			cmd.CommandText = "SELECT ua.IdSector,ua.IdPrefijo,ua.IdAbonado FROM usuariosAbonados ua, sectores s " +
									"WHERE s.IdSistema='" + IdSistema + "' AND s.SectorSimple AND " +
									"s.NumSacta IN (" + StringSectores.ToString() + ") AND " +
									"ua.IdSistema=s.IdSistema AND ua.IdNucleo=s.IdNucleo AND " +
									"ua.IdSector=s.IdSector";

			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					//string number = dr.GetString(1);

					if ((uint)dr["IdPrefijo"] == 3)	// ATS
					{
						string number = dr.GetString(2);

						if (number.Length > 0)
						{
							UserInfo user = null;

							if (ListOfUsersByName.TryGetValue(dr.GetString(0), out user))
							{
								user.ATSNumbers.Add(Int32.Parse(number));
							}
						}
					}
					else if (dr["IdAbonado"] != System.DBNull.Value)	// Publico
					{
						UserInfo user = null;

						if (ListOfUsersByName.TryGetValue(dr.GetString(0), out user))
						{
							UserInfo.StAbonado abonado;
							abonado.Prefijo = (uint)dr["IdPrefijo"];
							abonado.Numero = (string)dr["IdAbonado"];
							user.PublicNumbers.Add(abonado);
						}
					}
				}
			}
		}

		//void GetUsersAtsNumbers(DbCommand cmd)
		//{
		//    cmd.CommandText = "SELECT ua.IdSector,ua.IdAbonado FROM SectoresSectorizacion sz, SectoresSector ss, UsuariosAbonados ua " +
		//                        "WHERE sz.IdSistema='" + IdSistema + "' AND sz.IdSectorizacion='" + Name + "' AND ss.IdSector=sz.IdSector AND " +
		//                        "ua.IdSistema=ss.IdSistema AND ua.IdNucleo=ss.IdNucleo AND ua.IdSector=ss.IdSectorOriginal " +
		//                        "AND ua.IdPrefijo=3 ORDER BY ua.IdSector, ua.IdAbonado";
		//    using (DbDataReader dr = cmd.ExecuteReader())
		//    {
		//        while (dr.Read())
		//        {
		//            string number = dr.GetString(1);

		//            if (number.Length > 0)
		//            {
		//                UserInfo user = null;

		//                if (ListOfUsersByName.TryGetValue(dr.GetString(0), out user))
		//                {
		//                    user.ATSNumbers.Add(Int32.Parse(number));
		//                }
		//            }
		//        }
		//    }
		//}

		void GetUsersTlfColaterals(DbCommand cmd)
		{
			Dictionary<string, string> publicNumbers = new Dictionary<string, string>();

			// Los destinos ATS ademas de número R2 pueden tener número RTB (Público = 4)
			cmd.CommandText = "SELECT b.IdDestino,b.IdAbonado FROM DestinosTelefonia a,DestinosExternos b " +
								"WHERE a.IdSistema='" + IdSistema + "' AND a.IdSistema=b.IdSistema AND a.IdPrefijo=3 " +
								"AND a.IdDestino=b.IdDestino AND b.IdPrefijo<>3";
			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					string dst = dr.GetString(0);

					Debug.Assert(!publicNumbers.ContainsKey(dst));
					publicNumbers[dst] = dr.GetString(1);
				}
			}

			#region Destinos Externos de telefonía
			// Destinos de telefonía externos
			//cmd.CommandText = "SELECT a.* FROM DestinosExternosSector a, SectoresSectorizacion ss, SectoresSector s " +
			//                    "WHERE ss.IdSistema='" + IdSistema + "' AND ss.IdSectorizacion='" + Name + "' " +
			//                    "AND s.IdSistema=ss.IdSistema AND s.IdSector=ss.IdSector AND s.IdNucleo=ss.IdNucleo " +
			//                    "AND a.IdSistema=s.IdSistema AND a.IdSector=s.IdSectorOriginal " +
			//                    "AND a.IdNucleo=s.IdNucleo ORDER BY a.IdNucleo,a.IdSector,-a.PosHMI";
            cmd.CommandText = "SELECT a.*, d.IdGrupo FROM DestinosExternosSector a, Sectores s, DestinosTelefonia d " +
								"WHERE s.IdSistema='" + IdSistema + "' AND s.SectorSimple AND " +
										"s.NumSacta IN (" + StringSectores.ToString() + ") AND " +
										"a.IdSistema=s.IdSistema AND " +
                                        "d.IdSistema = a.IdSistema AND " +
                                        "d.IdDestino = a.IdDestino AND " +
                                        "d.TipoDestino = a.TipoDestino AND " +
                                        "d.IdPrefijo = a.IdPrefijo AND " +
                                        "a.IdSector=s.IdSector AND " +
										"a.IdNucleo=s.IdNucleo ORDER BY a.IdNucleo,a.IdSector,-a.PosHMI";
         
			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					UserInfo user = null;

					if (ListOfUsersByName.TryGetValue((string)dr["IdSector"], out user))
					{
						TLFEndPoint ep = new TLFEndPoint();

						ep.Position = (int)(uint)dr["PosHMI"];
                        ep.Origin = (string)dr["OrigenR2"];     //user.ATSNumbers[0].ToString(); // (string)dr["OrigenR2"];
						ep.PriorityR2 = (int)((uint)dr["PrioridadSIP"]);
						ep.Priority = (int)((uint)dr["Prioridad"]);
						ep.Name = (string)dr["IdDestino"];
						ep.Literal = (string)dr["Literal"];
						ep.Prefix = (PrefixType)((uint)dr["IdPrefijo"]);
						ep.IdDestino = (string)dr["IdDestino"];
						ep.TipoDestino = (uint)dr["TipoDestino"];
						ep.TipoAcceso = (string)dr["TipoAcceso"];

						if (ep.TipoAcceso == "DA")
						{
							if (ep.Position >= user.TlfEndPoints.Length)
								Array.Resize(ref user.TlfEndPoints, (int)(ep.Position + 1));
							user.TlfEndPoints[ep.Position] = ep;
						}
						else if (ep.TipoAcceso == "IA")
						{
                            if (dr["IdGrupo"] != System.DBNull.Value)
                                ep.Group = (string)dr["IdGrupo"];

                            //En el panel de LC sólo se pueden asignar destinos ATS
                            //comprobamos si el destino ATS, tiene asociado un recurso LCEN

                            if ((dr["IdDestinoLCEN"] != System.DBNull.Value) && (dr["IdPrefijoDestinoLCEN"] != System.DBNull.Value)
                                 &&  ((uint)dr["IdPrefijoDestinoLCEN"]== 1))
                            {
                                ep.strIdDestinoLCEN= (string) dr["IdDestinoLCEN"];
                            }

                            if (ep.Position >= user.LCEndPoints.Length)
								Array.Resize(ref user.LCEndPoints, (int)(ep.Position + 1));
							user.LCEndPoints[ep.Position] = ep;
						}
						else if (ep.TipoAcceso == "AG")
						{
							if (ep.Position >= user.AgEndPoints.Length)
								Array.Resize(ref user.AgEndPoints, (int)(ep.Position + 1));
							user.AgEndPoints[ep.Position] = ep;
						}

						if (ep.Prefix == PrefixType.ATS)
						{
							publicNumbers.TryGetValue(ep.Name, out ep.PublicNumber);
						}
					}
				}
			}
			#endregion	

			#region Destinos internos de telefonía
			// Destinos de telefonía internos
			//cmd.CommandText = "SELECT a.* FROM DestinosInternosSector a, SectoresSectorizacion ss, SectoresSector s " +
			//                    "WHERE ss.IdSistema='" + IdSistema + "' AND ss.IdSectorizacion='" + Name + "' " +
			//                    "AND s.IdSistema=ss.IdSistema AND s.IdSector=ss.IdSector AND s.IdNucleo=ss.IdNucleo " +
			//                    "AND a.IdSistema=s.IdSistema AND a.IdSector=s.IdSectorOriginal " +
			//                    "AND a.IdNucleo=s.IdNucleo ORDER BY a.IdNucleo,a.IdSector,-a.PosHMI";
			cmd.CommandText = "SELECT a.* FROM DestinosInternosSector a, Sectores s " +
								"WHERE s.IdSistema='" + IdSistema + "' AND s.SectorSimple AND " +
										"s.NumSacta IN (" + StringSectores.ToString() + ") AND " +
										"a.IdSistema=s.IdSistema AND a.IdSector=s.IdSector AND " +
										"a.IdNucleo=s.IdNucleo ORDER BY a.IdNucleo,a.IdSector,-a.PosHMI";

			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					UserInfo user = null;

					if (ListOfUsersByName.TryGetValue((string)dr["IdSector"], out user))
					{
						TLFEndPoint ep = new TLFEndPoint();

						ep.Position = (int)((uint)dr["PosHMI"]);
                        ep.Origin = user.ATSNumbers[0].ToString(); //(string)dr["OrigenR2"];
						ep.PriorityR2 = (int)((uint)dr["PrioridadSIP"]);
						ep.Priority = (int)((uint)dr["Prioridad"]);
						ep.Name = (string)dr["IdDestino"];
						ep.Literal = (string)dr["Literal"];
						ep.Prefix = (PrefixType)((uint)dr["IdPrefijo"]);
						ep.IdDestino = (string)dr["IdDestino"];
						ep.TipoDestino = (uint)dr["TipoDestino"];
						ep.TipoAcceso = (string)dr["TipoAcceso"];
						if (ep.TipoAcceso == "DA")
						{
							if (ep.Position >= user.TlfEndPoints.Length)
								Array.Resize(ref user.TlfEndPoints, (int)(ep.Position + 1));
							user.TlfEndPoints[ep.Position] = ep;
						}
						else if (ep.TipoAcceso == "IA")
						{
							if (ep.Position >= user.LCEndPoints.Length)
								Array.Resize(ref user.LCEndPoints, (int)(ep.Position + 1));
							user.LCEndPoints[ep.Position] = ep;
						}
						else if (ep.TipoAcceso == "AG")
						{
							if (ep.Position >= user.AgEndPoints.Length)
								Array.Resize(ref user.AgEndPoints, (int)(ep.Position + 1));
							user.AgEndPoints[ep.Position] = ep;
						}

						if (ep.Prefix == PrefixType.ATS)
						{
							publicNumbers.TryGetValue(ep.Name, out ep.PublicNumber);
						}
					}
				}
			}
			#endregion
		}

		private void GetUsersTlfAgenda(DbCommand cmd)
		{
			int posAgenda = 1;
			#region Agenda de telefonía
			cmd.CommandText = "SELECT a.* FROM Agenda a, Sectores s " +
								"WHERE a.IdSistema='" + IdSistema + "' AND s.SectorSimple AND " +
										"s.NumSacta IN (" + StringSectores.ToString() + ") AND " +
										"a.IdSistema=s.IdSistema AND a.IdSector=s.IdSector AND " +
										"a.IdNucleo=s.IdNucleo ORDER BY a.IdNucleo,a.IdSector,a.Nombre";

			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					UserInfo user = null;

					if (ListOfUsersByName.TryGetValue((string)dr["IdSector"], out user))
					{
						TLFEndPoint ep = new TLFEndPoint();

						ep.Position = posAgenda++;
						ep.Origin = (string)dr["IdSector"];
						ep.Name = (string)dr["Nombre"];
						ep.Literal = (string)dr["Nombre"];
						ep.Prefix = (PrefixType)((uint)dr["Prefijo"]);
						ep.IdDestino = (string)dr["Nombre"];
						ep.TipoAcceso = "AG";

						if (ep.Position >= user.AgEndPoints.Length)
							Array.Resize(ref user.AgEndPoints, (int)(ep.Position + 1));
						user.AgEndPoints[ep.Position] = ep;
					}
				}
			}
			#endregion
		}

		void GetUsersRdColaterals(DbCommand cmd)
		{
            // Localizar usuario fuera de sectorización (si lo hay)
            // para asignar los colaterales radio UNION del resto de usuarios.
            UserInfo userFS = null;
            bool hayUsuarioFS = ListOfUsersByName.TryGetValue(STR_SECTOR_FS, out userFS);
            int iNumRdFS = 0;
            System.Collections.Hashtable tlistaDestinosFS = new System.Collections.Hashtable();
            System.Collections.Hashtable tlistaUsuariosDestinosFS = new System.Collections.Hashtable();

            // Destinos de Radio
			cmd.CommandText = "SELECT a.* FROM DestinosRadioSector a, Sectores s " +
								"WHERE s.IdSistema='" + IdSistema + "' AND s.SectorSimple AND " +
										"s.NumSacta IN (" + StringSectores.ToString() + ") AND " +
										"a.IdSistema=s.IdSistema AND a.IdSector=s.IdSector AND " +
										"a.IdNucleo=s.IdNucleo ORDER BY a.IdNucleo,a.IdSector,-a.PosHMI";


			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					UserInfo user = null;

					if (ListOfUsersByName.TryGetValue((string)dr["IdSector"], out user))
					{
						RDEndPoint ep = new RDEndPoint();

						ep.Position = (int)((uint)dr["PosHMI"]);
						ep.Frecuency = (string)dr["IdDestino"];
						ep.Literal = (string)dr["Literal"];
						ep.Priority = (int)((uint)dr["Prioridad"]);
						ep.PrioridadSIP=(int)((uint)dr["PrioridadSIP"]);
						ep.EstadoAsignacion = (string)dr["ModoOperacion"];
                        ep.SupervisionPortadora = dr["SupervisionPortadora"] != System.DBNull.Value ? (bool)dr["SupervisionPortadora"] : false;
						if (ep.Position >= user.RdEndPoints.Length)
						{
							Array.Resize(ref user.RdEndPoints, (int)(ep.Position + 1));
						}

						user.RdEndPoints[ep.Position] = ep;

                        
                        if (hayUsuarioFS)
                        {
                            //Si no existe el destino radio en la sectorización FS, lo añadimos
                            if (!tlistaDestinosFS.ContainsKey(ep.Frecuency))
                            {
                                RDEndPoint epFS = new RDEndPoint();

                                iNumRdFS++;
                                epFS.Position = iNumRdFS;
                                epFS.Frecuency=ep.Frecuency;
                                epFS.Literal=ep.Literal;
                                epFS.Priority=ep.Priority;
                                epFS.PrioridadSIP=ep.PrioridadSIP;
                                epFS.EstadoAsignacion=ep.EstadoAsignacion;
                                epFS.SupervisionPortadora=ep.SupervisionPortadora;

                                if (iNumRdFS >= userFS.RdEndPoints.Length)
                                {
                                    Array.Resize(ref userFS.RdEndPoints, (int)(iNumRdFS + 1));
                                }

                                userFS.RdEndPoints[iNumRdFS] = epFS;
                                tlistaDestinosFS.Add(ep.Frecuency, iNumRdFS);
                                tlistaUsuariosDestinosFS.Add(ep.Frecuency, user.IdUsuario);
                            }
                        }
					}
				}
			}


			// Obtener el estado de los altavoces de cada posición.
			foreach (UserInfo user in ListOfUsersByName.Values)
			{
                if (user.IdUsuario != STR_SECTOR_FS && user.RdEndPoints.Length > 0)
                {
                    foreach (RDEndPoint rdPosicion in user.RdEndPoints)
                    {
                        if (rdPosicion != null)
                        {
                            rdPosicion.EstadoAltavoces = GetEstadoAltavoces(IdSistema, user.IdNucleo, rdPosicion.Frecuency, user.IdUsuario, (uint)rdPosicion.Position, cmd);
                            rdPosicion.EstadoRecursos = GetEstadoRecursos(IdSistema, user.IdNucleo, user.IdUsuario, (uint)rdPosicion.Position, cmd);
                            
                            if (hayUsuarioFS)
                            {
                                if (tlistaDestinosFS.ContainsKey(rdPosicion.Frecuency) && tlistaUsuariosDestinosFS.ContainsKey(rdPosicion.Frecuency))
                                {
                                    if (null != tlistaDestinosFS[rdPosicion.Frecuency])
                                    {
                                        int iPos = (int) tlistaDestinosFS[rdPosicion.Frecuency];
                                        string idUsuarioFS = (string)tlistaUsuariosDestinosFS[rdPosicion.Frecuency];

                                        if (null != userFS.RdEndPoints[iPos] && user.IdUsuario == idUsuarioFS)
                                        {
                                            userFS.RdEndPoints[iPos].EstadoAltavoces = rdPosicion.EstadoAltavoces;
                                            userFS.RdEndPoints[iPos].EstadoRecursos = rdPosicion.EstadoRecursos;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
			}


            if (tlistaDestinosFS.Count > 0)
                tlistaDestinosFS.Clear();

            if (tlistaUsuariosDestinosFS.Count > 0)
                tlistaUsuariosDestinosFS.Clear();

			//foreach (string userNum /* OJO, esto ha de ser un string */ in ListOfUsersByName.Keys)
			//{
			//    cmd.CommandText = "SELECT Posicion, Frecuencia, Emplazamiento, Prioritario FROM ColateralRd " +
			//        "WHERE IdConfig != 'ACTIVA' AND IdPanel = 7 AND NumUsuario = " + userNum + " ORDER BY Posicion";
			//    break;
			//}

			//using (DbDataReader dr = cmd.ExecuteReader())
			//{
			//    while (dr.Read())
			//    {
			//        RDEndPoint ep = new RDEndPoint();

			//        ep.Position = dr.GetInt32(0);
			//        ep.Frecuency = dr.GetString(1);
			//        ep.Place = dr.GetString(2);
			//        ep.Priority = dr.GetInt32(3);

			//        RdEndPoints.Add(ep);
			//    }
			//}
		}

		private Dictionary<string, string> GetEstadoRecursos(string idSistema, string idNucleo, string idSector, uint posHmi, DbCommand cmd)
		{
			Dictionary<string, string> estado = new Dictionary<string, string>();

			cmd.CommandText = "SELECT IdRecurso,Estado FROM EstadosRecursos " +
								"WHERE IdSistema='" + idSistema + "' AND " +
										"IdNucleo='" + idNucleo + "' AND " +
										"IdSector='" + idSector + "' AND " +
										"PosHMI=" + posHmi;

			using (DbDataReader dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					estado.Add((string)dr["IdRecurso"], (string)dr["Estado"]);
				}
			}

			return estado;
		}

		private List<string> GetEstadoAltavoces(string idSistema, string idNucleo, string idDestino, string idSector, uint posHmi, DbCommand comando)
		{
			List<string> listaAltavoces=new List<string>();

			comando.CommandText = "SELECT * FROM Altavoces " +
								"WHERE IdSistema='" + idSistema + "' AND " +
										"IdNucleo='" + idNucleo + "' AND " +
										"IdSector='" + idSector + "' AND " +
										"Estado<>'I' AND " + // Sólo los altavoces en estado de Seleccionado o Activo son sectorizables.
										"IdDestino='" + idDestino + "' AND " +
										"PosHMI=" + posHmi + " ORDER BY NumAltavoz";

			using (DbDataReader dr = comando.ExecuteReader())
			{
				while (dr.Read())
				{
					listaAltavoces.Add((string)dr["Estado"]);
				}
			}

			return listaAltavoces;
		}

		void RemoveMantSectors()
		{
			foreach (UcsInfo ucs in ListOfUcs.Values)
			{
				List<SectorInfo> mantSectors = ucs.Sectors.FindAll(delegate(SectorInfo obj) { return (obj.Type == "M"); });

				if (mantSectors.Count > 0)
				{
					if (mantSectors.Count < ucs.Sectors.Count)
					{
						foreach (SectorInfo sector in mantSectors)
						{
							ucs.Sectors.Remove(sector);
							ListOfSectors.Remove(sector.IdSector);
							if (sector.UserE != null) ListOfUsersByName.Remove(sector.UserE.IdUsuario);
							if (sector.UserP != null) ListOfUsersByName.Remove(sector.UserP.IdUsuario);
						}
					}
					else if (mantSectors.Count > 1)      // Todos los sectores son de mantenimiento y hay mas de uno
					{
						throw new SectorizationException(SectorizationResult.TwoMaintenanceSectors, Resources.MultipleMantSectorError);
					}
				}

				Debug.Assert(ucs.Sectors.Count > 0);
			}
		}

      #endregion

        /// <summary>
        /// AGL. Para poder seguir la ejecucion....
        /// </summary>
        /// <param name="isError"></param>
        /// <param name="from"></param>
        /// <param name="msg"></param>
        /// <param name="par"></param>
        void Log(bool isError, string from, string msg, params object[] par)
        {
            string message = String.Format("[{0}.{1}]: {2}", "SactaModule", from, msg);
            if (isError)
                NLog.LogManager.GetLogger("SactaModule").Error(message, par);
            else
                NLog.LogManager.GetLogger("SactaModule").Info(message, par);
        }

   }

}
