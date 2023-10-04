using Domain.Attributes;
using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities.Base;
using Domain.Query;

namespace Domain.Models.Validators.Base
{
    /// <summary>
    /// Class to validate other class
    /// </summary>
    public class EntityValidator<T> where T : Entity
    {
        public T Entity { get; private set; }

        public Fields<T> Fields { get; private set; }

        public ActionTypeEnum Action { get ; private set; }

        private void Init(T entity, Fields<T> fields, ActionTypeEnum action)
        {
            Entity = entity;
            Fields = fields;
            Action = action;
        }

        private bool InnerValidation(List<string> results, bool throwError)
        {
            var methods = GetType()
               .GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
               .Where(m => m.GetCustomAttributes(typeof(ValidatorAttribute), true).Length > 0)
               .ToList();

            bool isValid = true;

            foreach (var method in methods)
            {
                if (method.ReturnType == typeof(void))
                {
                    if (method.GetParameters().Count() == 0)
                    {
                        try
                        {
                            method.Invoke(this, null);
                        }
                        catch (Exception e)
                        {
                            if (e.InnerException != null && e.InnerException.GetType() == typeof(BusinessException))
                            {
                                string message = e.InnerException.Message;

                                if (message.Count() > 0)
                                {
                                    if (results != null)
                                        results?.Add(message);

                                    if (throwError)
                                        throw new BusinessException(message);

                                    if (results == null && throwError == false)
                                        return false;
                                    else
                                        isValid = false;
                                }
                            }
                            else
                            {
                                throw new InternalException($"Validaion method failed: {GetType().FullName}:{method.GetType().Name}.\n {e.Message}\n{e.InnerException?.Message}");
                            }
                        }
                    }
                    else
                    {
                        throw new InternalException($"Validation method can't have parameters: {GetType().FullName}:{method.GetType().Name}");
                    }
                }
                else
                {
                    throw new InternalException($"Validation method with invalid parameters: {GetType().FullName}:{method.GetType().Name}");
                }
            }

            return isValid;
        }

        public bool IsValid(T entity, ActionTypeEnum action, Fields<T> fields = null)
        {
            Init(entity, fields, action);

            return InnerValidation(null, false);
        }

        public void ValidateWithError(T entity, ActionTypeEnum action, Fields<T> fields = null)
        {
            Init(entity, fields, action);

            InnerValidation(null, true);
        }

        public List<string> ValidateWithMessages(T entity, ActionTypeEnum action, Fields<T> fields = null)
        {
            Init(entity, fields, action);

            List<string> messages = new List<string>();
            InnerValidation(messages, false);
            return messages;
        }
    }
}
