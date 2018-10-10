/*==============================================================================*/
/* include																		*/
/*==============================================================================*/
#include	<stdio.h>
#include	<stdlib.h>
/*==============================================================================*/
/* debug_out																	*/
/*==============================================================================*/
void	debug_out( char *f , const double d[] , int size , const char* ppath, int no )
{
	FILE		*fp;
	char		b[1024];
	errno_t error;

	sprintf_s( b , 1024, "%s\\%s(%d).txt" , ppath , f , no );

	error = fopen_s(&fp, b, "w");
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
/*==============================================================================*/
/* EOF */