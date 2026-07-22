# Control Académico Estudiantil

Aplicación móvil académica desarrollada para la asignatura **INF-4316 - Programación de Aplicaciones Móviles**.

Este repositorio corresponde al proyecto móvil del **Grupo #6** y su desarrollo se organiza por unidades mediante ramas de Git.

---

## Integrantes

- **Junior Alexis Valera Rijo**  
  Matrícula: **MT-2023-00518**

- **Yisel del Carmen Santana Rosario**  
  Matrícula: **SD-18-11014**

---

## Rama actual

```text
unidad-5
```

La rama **unidad-5** corresponde a la:

**Actividad Práctica 5 - Editar y eliminar registros**

---

## Objetivo de la Unidad 5

Completar las operaciones CRUD de la aplicación agregando la edición y eliminación de registros en los módulos académicos.

Los mismos formularios se utilizan tanto para crear registros nuevos como para editar registros existentes. El comportamiento, los títulos y los botones cambian dinámicamente según el modo activo.

---

## Descripción de la aplicación

**Control Académico Estudiantil** es una aplicación móvil desarrollada con **.NET MAUI** para administrar información académica básica desde un dispositivo Android o Windows.

La aplicación permite trabajar con los siguientes módulos:

- Estudiantes
- Materias
- Asistencias
- Calificaciones

Los datos se manejan temporalmente en memoria mediante la clase `ServicioAcademico`.

---

## Funcionalidades generales

- Inicio de sesión con credenciales académicas de prueba.
- Navegación mediante Shell.
- Menú lateral para acceder a los módulos.
- Opción para cerrar sesión.
- Panel principal adaptado a la Unidad 5.
- Diseño visual uniforme inspirado en los colores institucionales.
- Contadores dinámicos de registros.
- Mensajes de éxito después de guardar, editar o eliminar.
- Confirmación obligatoria antes de eliminar.
- Formularios con validaciones debajo de cada campo.
- Limpieza automática al regresar al modo creación.

---

## CRUD implementado

La aplicación permite realizar las cuatro operaciones principales:

| Operación | Descripción |
|---|---|
| Crear | Registrar nuevos datos desde el formulario |
| Consultar | Visualizar los registros en listas |
| Editar | Cargar un registro existente y guardar sus cambios |
| Eliminar | Borrar registros después de solicitar confirmación |

---

## Formularios dinámicos

Cada módulo utiliza un mismo formulario para crear y editar.

Cuando no existe un registro seleccionado, el formulario trabaja en modo creación:

- Nuevo estudiante
- Nueva materia
- Nueva asistencia
- Nueva calificación

Cuando se selecciona una tarjeta, el formulario cambia a modo edición:

- Editar estudiante
- Editar materia
- Editar asistencia
- Editar calificación

En modo edición:

- Los campos se cargan automáticamente.
- El botón principal cambia a **Guardar cambios**.
- El botón secundario cambia a **Cancelar edición**.
- Aparece un botón adicional para eliminar el registro.
- Se muestra el indicador visual **Editando**.

---

## Eliminación de registros

Cada registro puede eliminarse desde dos lugares:

- Desde el botón pequeño **Eliminar** ubicado en su tarjeta.
- Desde el botón grande del formulario cuando está activo el modo edición.

Antes de borrar, la aplicación presenta un diálogo con las opciones:

- Cancelar
- Eliminar

El registro solo se elimina cuando el usuario confirma la acción.

---

## Módulo de Estudiantes

Permite administrar:

- Matrícula
- Nombre
- Apellido
- Carrera
- Teléfono

Validaciones principales:

- Campos obligatorios.
- Formato de matrícula.
- Matrículas no duplicadas.
- Exclusión del propio registro durante la edición.
- Validación del teléfono.

---

## Módulo de Materias

Permite administrar:

- Código
- Nombre de la asignatura
- Profesor
- Cantidad de créditos

Validaciones principales:

- Campos obligatorios.
- Códigos de materia no duplicados.
- Exclusión del propio registro durante la edición.
- Créditos expresados como número válido.

---

## Módulo de Asistencias

Permite administrar:

- Estudiante
- Materia
- Fecha
- Estado de asistencia

Estados disponibles:

- Presente
- Ausente
- Excusa

Validaciones principales:

- Selección obligatoria de estudiante, materia y estado.
- La fecha no puede ser futura.
- No se permite repetir estudiante, materia y fecha.
- El propio registro se excluye al editar.

---

## Módulo de Calificaciones

Permite administrar:

- Estudiante
- Materia
- Nota
- Observación

Validaciones principales:

- Selección obligatoria de estudiante y materia.
- Nota numérica entre 0 y 100.
- Observación de hasta 120 caracteres.
- Una calificación por estudiante y materia.
- Exclusión del propio registro durante la edición.

---

