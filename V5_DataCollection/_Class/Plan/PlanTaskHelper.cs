/*
CronTrigger���ø�ʽ:

��ʽ: [��] [��] [Сʱ] [��] [��] [��] [��]

 ���	˵�� 
 �Ƿ����	 ������д��ֵ	�����ͨ��� 
 1	 ��	 ��	 0-59 	  , - * /
 2	 ��	 ��	 0-59 
  , - * /
 3	Сʱ	 ��	 0-23	  , - * /
 4	 ��	 ��	 1-31	  , - * ? / L W
 5	 ��	 ��	 1-12 or JAN-DEC	  , - * /
 6	 ��	 ��	 1-7 or SUN-SAT	  , - * ? / L #
 7	 ��	 ��	 empty �� 1970-2099	 , - * /
ͨ���˵��:
* ��ʾ����ֵ. ����:�ڷֵ��ֶ������� "*",��ʾÿһ���Ӷ��ᴥ����
? ��ʾ��ָ��ֵ��ʹ�õĳ���Ϊ����Ҫ���ĵ�ǰ��������ֶε�ֵ������:Ҫ��ÿ�µ�10�Ŵ���һ�������������������ܼ���������Ҫ��λ�õ��Ǹ��ֶ�����Ϊ"?" ��������Ϊ 0 0 0 10 * ?
- ��ʾ���䡣���� ��Сʱ������ "10-12",��ʾ 10,11,12�㶼�ᴥ����
, ��ʾָ�����ֵ�����������ֶ������� "MON,WED,FRI" ��ʾ��һ�����������崥��
/ ���ڵ�����������������������"5/15" ��ʾ��5�뿪ʼ��ÿ��15�봥��(5,20,35,50)�� �����ֶ�������'1/3'��ʾÿ��1�ſ�ʼ��ÿ�����촥��һ�Ρ�
L ��ʾ������˼�������ֶ������ϣ���ʾ���µ����һ��(���ݵ�ǰ�·ݣ�����Ƕ��»��������Ƿ�������[leap]), �����ֶ��ϱ�ʾ���������൱��"7"��"SAT"�������"L"ǰ�������֣����ʾ�����ݵ����һ�������������ֶ�������"6L"�����ĸ�ʽ,���ʾ���������һ��������" 
W ��ʾ��ָ�����ڵ�����Ǹ�������(��һ������). ���������ֶ�������"15W"����ʾ��ÿ��15��������Ǹ������մ��������15���������������������������(14��)����, ���15������δ���������������һ(16��)����.���15�������ڹ�����(��һ������)������ڸ��촥�������ָ����ʽΪ "1W",�����ʾÿ��1����������Ĺ����մ��������1����������������3������һ������(ע��"W"ǰֻ�����þ��������,����������"-").
С��ʾ
'L'�� 'W'����һ���ʹ�á���������ֶ�������"LW",���ʾ�ڱ��µ����һ�������մ���(һ��ָ������ ) 

# ���(��ʾÿ�µĵڼ����ܼ�)�����������ֶ�������"6#3"��ʾ��ÿ�µĵ���������.ע�����ָ��"#5",���õ�����û���������򲻻ᴥ��������(����ĸ�׽ں͸��׽��ٺ��ʲ�����)
С��ʾ
���ֶε����ã���ʹ��Ӣ����ĸ�ǲ����ִ�Сд�� MON ��mon��ͬ.


        
����ʾ��:
  
0 0 12 * * ?	ÿ��12�㴥��
0 15 10 ? * *	ÿ��10��15�ִ���
0 15 10 * * ?	ÿ��10��15�ִ���
0 15 10 * * ? *	ÿ��10��15�ִ���
0 15 10 * * ? 2005	2005��ÿ��10��15�ִ���
0 * 14 * * ?	ÿ������� 2�㵽2��59��ÿ�ִ���
0 0/5 14 * * ?	ÿ������� 2�㵽2��59��(���㿪ʼ��ÿ��5�ִ���)
0 0/5 14,18 * * ?	ÿ������� 2�㵽2��59��(���㿪ʼ��ÿ��5�ִ���)
ÿ������� 18�㵽18��59��(���㿪ʼ��ÿ��5�ִ���)
0 0-5 14 * * ?	ÿ������� 2�㵽2��05��ÿ�ִ���
0 10,44 14 ? 3 WED	3�·�ÿ��������� 2��10�ֺ�2��44�ִ���
0 15 10 ? * MON-FRI	����һ������ÿ�������10��15�ִ���
0 15 10 15 * ?	ÿ��15������10��15�ִ���
0 15 10 L * ?	ÿ�����һ���10��15�ִ���
0 15 10 ? * 6L	ÿ�����һ�ܵ��������10��15�ִ���
0 15 10 ? * 6L 2002-2005	��2002�굽2005��ÿ�����һ�ܵ��������10��15�ִ���
0 15 10 ? * 6#3	ÿ�µĵ����ܵ������忪ʼ����
0 0 12 1/5 * ?	ÿ�µĵ�һ�����翪ʼÿ��5�촥��һ��
0 11 11 11 11 ?	ÿ���11��11�� 11��11�ִ���(�����)
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using V5_DataCollection._Class.DAL;
using V5_Model;

namespace V5_DataCollection._Class.Plan {
    /// <summary>
    /// �ƻ�����
    /// </summary>
    public class PlanTaskHelper {

        static DALTask dal = new DALTask();

        private static IScheduler scheduler;
        /// <summary>
        /// ��ʼ��һ��������
        /// </summary>
        public static void InitScheduler() {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
        }

        /// <summary>
        /// ����������ҵ
        /// </summary>
        public static void LoadAllJobs() {
            DataTable dt = dal.GetList(" And IsPlan=1 ").Tables[0];
            //DataTable dt = dal.GetList("Order By Id Asc").Tables[0];
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    #region Job
                    var job = dal.GetModel(int.Parse(dr["Id"].ToString()));
                    if (string.IsNullOrEmpty(job.PlanFormat)) {
                        continue;
                    }
                    var jobNum = string.Format("job_{0}", dr["Id"].ToString());
                    if (scheduler.CheckExists(new JobKey(jobNum))) {
                        continue;
                    }
                    var dic3 = new Dictionary<string, ModelTask>();
                    dic3.Add(jobNum, job);

                    IJobDetail job3 = JobBuilder.Create<JobDetailHelper>()
                        .WithIdentity(jobNum)
                        .SetJobData(new JobDataMap(dic3))
                        .Build();
                    #endregion

                    #region ������
                    Quartz.Collection.ISet<ITrigger> ll = new Quartz.Collection.HashSet<ITrigger>();

                    var triggerNum = string.Format("trigger_{0}", dr["Id"].ToString());
                    var t3 = TriggerBuilder.Create();
                    t3.WithIdentity(triggerNum);
                    t3.WithCronSchedule(job.PlanFormat);//"0/5 * * * * ?"
                    var trigger3 = t3.Build();
                    //scheduler.ScheduleJob(job3, trigger3);
                    ll.Add(trigger3);
                    #endregion
                    scheduler.ScheduleJob(job3, ll, true);
                }
            }
        }

        /// <summary>
        /// Pushһ����ҵ
        /// </summary>
        /// <param name="model"></param>
        public static void PushJobDetail(int taskId) {

            #region Job
            var job = dal.GetModel(taskId);
            var jobNum = string.Format("job_{0}", taskId);
            if (scheduler.CheckExists(new JobKey(jobNum))) {
                scheduler.DeleteJob(new JobKey(jobNum));
            }
            var dic3 = new Dictionary<string, ModelTask>();
            dic3.Add(jobNum, job);

            IJobDetail job3 = JobBuilder.Create<JobDetailHelper>()
                .WithIdentity(jobNum)
                .SetJobData(new JobDataMap(dic3))
                .Build();
            #endregion

            #region ������
            Quartz.Collection.ISet<ITrigger> ll = new Quartz.Collection.HashSet<ITrigger>();
            //��ӵ���������
            var triggerNum = string.Format("trigger_{0}", taskId);
            var t3 = TriggerBuilder.Create();
            t3.WithIdentity(triggerNum);
            t3.WithCronSchedule(job.PlanFormat);//"0/5 * * * * ?"
            var trigger3 = t3.Build();
            //scheduler.ScheduleJob(job3, trigger3);
            ll.Add(trigger3);
            #endregion

            scheduler.ScheduleJob(job3, ll, true);
        }

        /// <summary>
        ///ɾ��һ����ҵ 
        /// </summary>
        /// <param name="model"></param>
        public static void DelJobDetail(int jobId) {
            JobKey jobK = new JobKey("job_" + jobId);
            if (scheduler.CheckExists(jobK)) {
                scheduler.DeleteJob(jobK);
            }
        }
    }
}
