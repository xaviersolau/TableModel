# Table server

## Ownership

|                   | Client | Server |
|-------------------|--------|-----|
| Table structure   | Can define locally | Own |
| Table decorator   | Can define locally | Own |
| Table sort        | Own    |     |
| Table filter      | Own    |     |

## Endpoint:

Tables
* Get Table list

Tables / Table id
* Get (Table filter, Table sort)

Process data query with filter sort and page directives.
Return ```IEnumerable<TData>```

Tables / Table id / Structure
* Get structure.

Tables / Table id / Decorators
* Get Decorator list

Tables / Table id / Decorators / Decorator id
* Get Decorator

