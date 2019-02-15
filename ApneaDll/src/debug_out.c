/*==============================================================================*/
/* include																		*/
/*==============================================================================*/
#include	<stdio.h>
#include	<share.h>
#include	<stdlib.h>
/*==============================================================================*/
/* debug_out																	*/
/*==============================================================================*/
void	debug_out( char *f , double d[] , int size , const char* ppath)
{
	FILE		*fp;
	char		b[1024];

	sprintf_s( b , 1024, "%s\\%s.txt" , ppath, f );

	fp = _fsopen(b, "w", SH_DENYNO);
	if (fp == NULL)
	{
		printf("_fsopen failed\n");
		exit(1);
	}
	
	for(int i = 0; i < size; i++)
	{
		fprintf( fp , "%lf\n" , d[i] );
	}
	
	fclose(fp);
}
/*==============================================================================*/
/* EOF */