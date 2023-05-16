using API.Infra.Decorators;
using API.Infra.Exceptions;

namespace API.Infra.Base
{
    /// <summary>
    /// Class to turn an class validable
    /// </summary>
    public class Validable
    {
        private bool InnerValidation(List<string> results, bool throwError)
        {
            var methods = this.GetType()
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
                        catch(Exception e)
                        {
                            if(e.InnerException != null && e.InnerException.GetType() == typeof(BusinessException))
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
                                throw new InternalException($"Validaion method failed: {this.GetType().FullName}:{method.GetType().Name}.\n {e.Message}\n{e.InnerException?.Message}");
                            }
                        }
                    }
                    else
                    {
                        throw new InternalException($"Validation method can't have parameters: {this.GetType().FullName}:{method.GetType().Name}");
                    }
                }
                else
                {
                    throw new InternalException($"Validation method with invalid parameters: {this.GetType().FullName}:{method.GetType().Name}");
                }
            }

            return isValid;
        }

        public bool IsValid()
        {
            return InnerValidation(null, false);
        }

        public void ValidateWithError()
        {
            InnerValidation(null, true);
        }

        public List<string> ValidateWithMessages()
        {
            List<string> messages = new List<string>();
            InnerValidation(messages, false);
            return messages;
        }
    }
}
