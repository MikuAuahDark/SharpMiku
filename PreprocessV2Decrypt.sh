# Requires GCC and dos2unix. Any GCC version is acceptable

printf "// This file is preprocessed by GCC\n\n" > src/DecryptionV2.cs
gcc -x c -E src/DecryptionV2_Unpreprocessed.cs -o temp.cs
dos2unix temp.cs
tail -c +146 temp.cs >> src/DecryptionV2.cs
rm temp.cs
