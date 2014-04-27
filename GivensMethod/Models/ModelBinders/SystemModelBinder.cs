using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GivensMethod.Models.ModelBinders
{
    public class SystemModelBinder : DefaultModelBinder
    {
        private const string DOUBLE_ERR = "Sistemul trebuie sa contina numai valori din R";
        SystemModel model = new SystemModel();

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!bindId(bindingContext))
            {
                return model;
            }

            if (!bindA(bindingContext))
            {
                return model;
            }

            if (!bindB(bindingContext))
            {
                return model;
            }

            return model;
        }

        private bool bindA(ModelBindingContext bindingContext)
        {
            int numRows = 0;
            while (null != bindingContext.ValueProvider.GetValue(String.Format("a[{0}]", numRows)))
            {
                numRows++;
            }

            ValueProviderResult[] lines = new ValueProviderResult[numRows];

            for (int i = 0; i < numRows; i++)
            {
                lines[i] = bindingContext.ValueProvider.GetValue(String.Format("a[{0}]", i));
            }

            if (lines.Length <= 0)
            {
                bindingContext.ModelState.AddModelError("A", "Matricea coeficienților nu este o matrice!");
                return false;
            }

            int numCols = ((string[])lines[0].RawValue).Length;

            double[,] a = new double[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                string[] formValues = ((string[])lines[i].RawValue);
                numCols = formValues.Length;

                if (numRows != numCols || numCols == 0)
                {
                    bindingContext.ModelState.AddModelError("A", "Matricea coeficienților nu este o matrice pătratică!");
                    return false;
                }

                for (int j = 0; j < numCols; j++)
                {
                    double val;
                    bool ok;
                    ok = Double.TryParse(formValues[j], out val);
                    if (!ok)
                    {
                        bindingContext.ModelState.AddModelError("Double", DOUBLE_ERR);
                        return false;
                    }

                    a[i, j] = val;
                }
            }

            model.a = a;

            return true;
        }

        private bool bindB(ModelBindingContext bindingContext)
        {
            int numRows = 0;
            while (null != bindingContext.ValueProvider.GetValue(String.Format("b[{0}]", numRows)))
            {
                numRows++;
            }

            if (numRows == 0)
            {
                bindingContext.ModelState.AddModelError("A", "Matricea termenilor liberi nu este o matrice!");
                return false;
            }

            double[] b = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                bool ok;
                double val;

                string formValue = ((string[])bindingContext.ValueProvider.GetValue(String.Format("b[{0}]", i)).RawValue)[0];

                ok = Double.TryParse(formValue, out val);
                if (!ok)
                {
                    bindingContext.ModelState.AddModelError("Double", DOUBLE_ERR);
                    return false;
                }

                b[i] = val;
            }

            model.b = b;
            return true;
        }

        private bool bindId(ModelBindingContext bindingContext)
        {
            string id = ((string[])bindingContext.ValueProvider.GetValue("id").RawValue)[0];
            if (null == id || "" == id)
            {
                bindingContext.ModelState.AddModelError("ID", "Numele sistemului trebuie să conțină o valoare!");
                return false;
            }

            model.id = id;
            return true;
        }
    }

}