using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ESBasic.ObjectManagement.Forms
{
    /// <summary>
    /// FormManager 用于管理同一类型的多个Form实例。FormManager是线程安全的。
    /// zhuweisky 2010.08.07
    /// </summary>
    /// <typeparam name="TFormID">每个Form实例的唯一标识的类型</typeparam>
    /// <typeparam name="TForm">Form实例的类型</typeparam>
    public class FormManager<TFormID, TForm> where TForm : Form, IManagedForm<TFormID>
    {
        private Dictionary<TFormID, TForm> formDictionary = new Dictionary<TFormID, TForm>();
        private object locker = new object();
        public event CbGeneric<TFormID> FormClosed;

        #region Ctor
        public FormManager()
        {
            this.FormClosed += delegate { };
        } 
        #endregion

        #region Add
        public void Add(TForm form)
        {
            lock (this.locker)
            {
                if (this.formDictionary.ContainsKey(form.FormID))
                {
                    TForm old = this.formDictionary[form.FormID];
                    old.FormClosed -= new FormClosedEventHandler(form_FormClosed);
                    this.formDictionary.Remove(form.FormID);
                }

                form.FormClosed += new FormClosedEventHandler(form_FormClosed);
                this.formDictionary.Add(form.FormID, form);
            }
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (this.locker)
            {
                TForm form = (TForm)sender;
                if (this.formDictionary.ContainsKey(form.FormID) && form == this.formDictionary[form.FormID])
                {                   
                    this.formDictionary.Remove(form.FormID);
                    this.FormClosed(form.FormID);
                }
            }
        }
        #endregion

        #region GetForm
        public TForm GetForm(TFormID id)
        {
            if (this.formDictionary.ContainsKey(id))
            {
                return this.formDictionary[id];
            }

            return null;
        } 
        #endregion        

        #region GetAllForms
        public List<TForm> GetAllForms()
        {
            lock (this.locker)
            {
                return new List<TForm>(this.formDictionary.Values);
            }
        } 
        #endregion

        #region Contains
        public bool Contains(TFormID id)
        {
            return this.formDictionary.ContainsKey(id);
        } 
        #endregion       
    }

    /// <summary>
    /// IManagedForm 被管理的Form实例的类型必须从IManagedForm继承。
    /// </summary>
    /// <typeparam name="TFormID">每个Form实例的唯一标识的类型</typeparam>
    public interface IManagedForm<TFormID>
    {
        TFormID FormID { get; }
    }
}
