import { Dialog } from "primereact/dialog";
import { FormikErrors, useFormik } from "formik";
import RoleService from "../../services/role/role.services";
import { InputText } from "primereact/inputtext";
import { classNames } from "primereact/utils";
import FormikValueIsValid from "../../utils/FormikValueIsValid";
import { Button } from "primereact/button";
import { useToast } from "../../utils/toast";
import { RoleDto, UpdateRoleDto } from "../../services/role/role.dto";
import { t } from "i18next";
import { MultiSelect, MultiSelectChangeEvent } from "primereact/multiselect";
import { useQuery } from "@tanstack/react-query";

const UpsertRole = (props: any) => {
  const role: RoleDto = props.role;
  const setRole = props.setRole;
  const visible: boolean = props.visible;
  const setVisible = props.setVisible;
  const refetchRole = props.refetchRole;
  const toast = useToast();

  const { data: permissionsResponse } = useQuery({
    queryKey: ["Permissions"],
    refetchOnMount: true,
    queryFn: () => RoleService.GetAllPermissions(),
  });

  let updateRoleDto: UpdateRoleDto;

  if (role === null || role === undefined) {
    updateRoleDto = {
      id: "",
      name: "",
      permissionIds: [],
    };
  } else {
    updateRoleDto = {
      ...role,
      permissionIds: role.permissions.map((x) => x.id),
    };
  }

  const formik = useFormik<UpdateRoleDto>({
    initialValues: updateRoleDto,
    enableReinitialize: true,
    validate: (data) => {
      let errors: FormikErrors<UpdateRoleDto> = {};
      if (!data.name) {
        errors.name = t("RoleNameCannotBeEmpty");
      }

      if (!data.permissionIds || data.permissionIds.length === 0) {
        errors.permissionIds = t("PermissionCannotBeEmpty");
      }

      return errors;
    },
    onSubmit: () => {
      var roleUpsertDto: UpdateRoleDto = {
        ...formik.values,
      };

      if (roleUpsertDto.id === "") {
        RoleService.Create(roleUpsertDto).then((res: any) => {
          if (!res.error) {
            setVisible(false);
            formik.resetForm();
            setRole(undefined);
            refetchRole();
            toast.show(t("RoleAddedSuccessfully"), "success");
          }
        });
      } else {
        RoleService.Update(roleUpsertDto).then((res: any) => {
          if (!res.error) {
            setVisible(false);
            formik.resetForm();
            setRole(undefined);
            refetchRole();
            toast.show(t("RoleUpdatedSuccessfully"), "success");
          }
        });
      }
    },
  });

  return (
    <>
      <Dialog
        visible={visible}
        modal={true}
        header={formik.values.id === "" ? t("AddRole") : t("UpdateRole")}
        onHide={() => {
          setVisible(false);
          formik.resetForm();
          setRole(undefined);
        }}
        footer={
          <div className="flex flex-row gap-3 justify-content-center md:justify-content-end mt-5">
            <Button
              label={t("Cancel")}
              className="md:px-6 justify-content-center"
              type="button"
              severity="secondary"
              onClick={() => {
                setVisible(false);
                formik.resetForm();
                setRole(undefined);
              }}
            />
            <Button
              label={t("Save")}
              className="md:px-6 justify-content-center"
              onClick={() => formik.submitForm()}
            />
          </div>
        }
        className="w-9 md:w-6"
      >
        <div className="grid mt-1">
          <div className="col-12">
            <label htmlFor="name">{t("Name")}</label>
            <InputText
              id="name"
              name="name"
              value={formik.values.name}
              onChange={(e) => {
                formik.setFieldValue("name", e.target.value);
              }}
              className={classNames("w-full mt-2", {
                "p-invalid": FormikValueIsValid(formik, "name"),
              })}
            />
            {FormikValueIsValid(formik, "name") && (
              <div className="p-error mt-3">{formik.errors.name}</div>
            )}
          </div>

          <div className="col-12">
            <label htmlFor="permissionIds">{t("Permissions")}</label>
            <MultiSelect
              id="permissionIds"
              name="permissionIds"
              display="chip"
              filter
              value={formik.values.permissionIds}
              onChange={(e: MultiSelectChangeEvent) => {
                formik.setFieldValue("permissionIds", e.value);
              }}
              options={permissionsResponse?.result?.data}
              optionLabel="name"
              optionValue="id"
              placeholder={t("SelectPermissions")}
              className={classNames("w-full mt-2", {
                "p-invalid": FormikValueIsValid(formik, "permissionIds"),
              })}
            />
            {FormikValueIsValid(formik, "permissionIds") && (
              <div className="p-error mt-3">{formik.errors.permissionIds}</div>
            )}
          </div>
        </div>
      </Dialog>
    </>
  );
};

export default UpsertRole;
