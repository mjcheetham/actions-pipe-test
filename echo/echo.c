#include <stdlib.h>
#include <stdio.h>
#include <io.h>

int main(int argc, char **argv)
{
    char buf[256];
    int nr = 0;
    while ((nr = _read(0, &buf, 1)) > 0)
    {
        buf[nr] = '\0';
        printf("%s", &buf);
    }
}
