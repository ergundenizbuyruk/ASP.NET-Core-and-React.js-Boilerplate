const FormikValueIsValid = (formik: any, fieldName: string) => {
  return formik.touched[fieldName] && formik.errors[fieldName];
};

export default FormikValueIsValid;
