/*==============================================================================*/
/* include																		*/
/*==============================================================================*/
#include	<stdio.h>
#include	<stdlib.h>
/*==============================================================================*/
/* debug_out																	*/
/*==============================================================================*/
void	debug_out( char *f , double d[] , int size , const char* ppath)
{
	FILE		*fp;
	char		b[1024];
	errno_t error;

	sprintf_s( b , 1024, "%s\\%s.txt" , ppath, f );

	error = fopen_s(&fp, b,  "w" );
	if (error != 0)
	{
		printf("file open error [debug_out]\n");
		exit(0);
	}

	for(int i = 0; i < size; i++)
	{
		fprintf( fp , "%lf\n" , d[i] );
	}
	fclose( fp );
}

void	debug_out_add(char *f, double d[], int size, const char* ppath)
{
	FILE		*fp;
	char		b[1024];
	errno_t error;

	sprintf_s(b, 1024, "%s\\%s.txt", ppath, f);

	error = fopen_s(&fp, b, "a");
	if (error != 0)
	{
		printf("file open error [debug_out]\n");
		exit(0);
	}

	for (int i = 0; i < size; i++)
	{
		fprintf(fp, "%lf\n", d[i]);
	}
	fclose(fp);
}
/*==============================================================================*/
/* EOF */