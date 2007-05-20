using System.Collections.Generic;
using System.Threading;

namespace Ferda.Guha.MiningProcessor
{
	public class MiningSettingCollectionWithMiningThreads<Type>
	{
		private Queue<Type> miningSetting = new Queue<Type>();
		private Mine mine;
		private Finished finished;
		private Semaphore semaphore;
		private bool runThreads = true;
		private List<Thread> threads = new List<Thread>();
		
		public delegate bool Finished();
		public delegate void Mine(Type t);
		
		public MiningSettingCollectionWithMiningThreads(Mine m, Finished f)
		{
			mine = m;
			finished = f;
			semaphore = new Semaphore(0, 2 * System.Environment.ProcessorCount);
			for(int i = 0; i < System.Environment.ProcessorCount; i++)
			{
				Thread thread = new Thread(threadDo);
				thread.Start();
				threads.Add(thread);
			}
		}
		
		private void threadDo()
		{
			bool runThreadsLocal;
			
			do
			{
				semaphore.WaitOne();
				Type t;
				lock (this)
				{
					lock (miningSetting)
					{
						runThreadsLocal = (runThreads || miningSetting.Count > 0) && !finished();
						if(runThreadsLocal)
						{
							t = miningSetting.Dequeue();
						}
						else
						{
							continue;
						}
					}
				}
				mine(t);
			}
			while(runThreadsLocal);
		}
		
		public void AddSetting(Type t)
		{
			lock(miningSetting)
			{
				miningSetting.Enqueue(t);
			}
		}
		
		public void Finish()
		{
			lock(this)
			{
				runThreads = false;
			}
			for(int i = 0; i < System.Environment.ProcessorCount; i++)
			{
				semaphore.Release();
			}
			
			foreach(Thread thread in threads)
			{
				thread.Join();
			}
		}
	}
}
