using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Timers;

namespace UtilitiesCD40
{
	public delegate void TransactionEventHandler();
	
	public class TimeoutTransactionException : Exception
	{
		public TimeoutTransactionException(string msg)
			: base(msg)
		{
		}
	}

	public sealed class TimeoutTransaction 
	{
		public event TransactionEventHandler EventTransaction;
		private Timer ElapsedTimeoutTransaction;
		private long MomentoDeInicio = 0;
		private static int NumeroSavePoint = 0;
		private const int MaxSavePoint = 10000;

		private MySqlTransaction _MyTransaction;
		public MySqlTransaction MyTransaction
		{
			get { return _MyTransaction; }
			set { _MyTransaction = value; }
		}

		public TimeoutTransaction(double time)
		{
			if (time != 0)	// Si 0 la transacción no vence
			{
				MomentoDeInicio = (long)(DateTime.Now.Ticks / 10E6);
				ElapsedTimeoutTransaction = new Timer(time);
				ElapsedTimeoutTransaction.Enabled = false;
				ElapsedTimeoutTransaction.AutoReset = true;
				ElapsedTimeoutTransaction.Elapsed += new ElapsedEventHandler(TransactionTimeouted);
			}
		}

		public int TimeRemaining()
		{
			if (MomentoDeInicio == 0 || !ElapsedTimeoutTransaction.Enabled)
				return 0;

			return (int)((ElapsedTimeoutTransaction.Interval / 1000) - (double)((DateTime.Now.Ticks / 10E6) - MomentoDeInicio));
		}

		void TransactionTimeouted(object sender, ElapsedEventArgs e)
		{
			EventTransaction();
		}

		public void ResetTimeout()
		{
			if (MomentoDeInicio != 0)
			{
				ElapsedTimeoutTransaction.Enabled = true;
				MomentoDeInicio = (long)(DateTime.Now.Ticks / 10E6);
				//ElapsedTimeoutTransaction.Stop();
				//ElapsedTimeoutTransaction.Start();
			}
		}

		public void Commit()
		{
			if (MyTransaction != null)
			{
				try
				{
					MyTransaction.Commit();
				}
				catch (Exception)
				{
					Rollback();
				}
				finally
				{
					if (ElapsedTimeoutTransaction != null)
						ElapsedTimeoutTransaction.Enabled = false;
				}
			}
		}

		public void Rollback()
		{
			try
			{
				MyTransaction.Rollback();
			}
			catch (MySqlException mEx)
			{

			}
			catch (System.InvalidOperationException e)
			{
			}
			catch (Exception e)
			{
				
			}
			finally
			{
				if (ElapsedTimeoutTransaction != null)
					ElapsedTimeoutTransaction.Enabled = false;
			}
		}

		public string Savepoint(MySqlCommand command)
		{
			ResetTimeout();

			string nameSavePoint = "punto" + (NumeroSavePoint++ % MaxSavePoint);
			command.CommandText = "SAVEPOINT " + nameSavePoint;
			try
			{
				command.ExecuteNonQuery();
			}
			catch (MySqlException mEx)
			{
				if (command.Connection.State== System.Data.ConnectionState.Closed ||
					command.Connection.State== System.Data.ConnectionState.Broken)
				return "";
			}
			catch (System.InvalidOperationException e)
			{
				if (command.Connection.State== System.Data.ConnectionState.Closed ||
					command.Connection.State== System.Data.ConnectionState.Broken)
				return string.Empty;
			}

			return nameSavePoint;
		}

		public void Rollback(string nameSavePoint, MySqlCommand command)
		{
			command.CommandText = "ROLLBACK TO SAVEPOINT " + nameSavePoint;
			try
			{
				command.ExecuteNonQuery();
			}
			catch (MySqlException mEx)
			{

			}
			catch (System.InvalidOperationException e)
			{
			}
			catch (Exception e)
			{

			}
		}
	}
}
