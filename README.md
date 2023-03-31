# DataEncryptProtection

Project created in order to encrypt data (ex: password) based on the current user account.

```pwsh
DataProtector.exe -o <output-file-path> --salt <salt> <data-to-encrypt>
```


Exemple:
```pwsh
DataProtector.exe -o output.txt --salt my-random-salt hello world

####### Result file output.txt
# Date: 31/03/2023 12:46:31
# User: admin
# Salt: my-random-salt

AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAQRhUQS729k6Zv4ypsDzw/AAAAAACAAAAAAAQZgAAAAEAACAAAACENEE5O7cRqNUlIR4z8xvhnfj/ArUogIaP4CEy9WnPbQAAAAAOgAAAAAIAACAAAABKHskqmiHoianFe89JPtwf8zHy66MnS328CnMRm19zLxAAAACwXMUOClpxhoeDN3OMQTFmQAAAAEUrRV5q9SvuJam8A1wubPfhd33nQgsfwXWbTh7atqkZB8kwkCPHFNoEQk8X3uZ/JBSyhxSdIfNsIQqaRe7NThg=

```