# Presentational Components
- Mainly concerned with how things look.
- Have no major dependencies on the rest of the app.
- No connection with the specification of the data that is outside the component.
- Mainly receives data and callbacks exclusively via props.
- May contain both presentational and container components inside it considering the fact that it contains DOM markup and styles of their own.

**Example**: For example, the below code denotes a presentational component, and it gets its data from the props and just focuses on showing an element. 