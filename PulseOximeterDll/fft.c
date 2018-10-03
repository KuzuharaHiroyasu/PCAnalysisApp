﻿/**

	パワースペクトル—スペクトルを測定し、結果をパワーで表示します。
	すべての位相情報は計算中に失われます。 
	通常、この測定は1つの信号の様々な周波数要素を調べるために使用します。
	パワースペクトルを求める平均はシステムの不要なノイズを削減しませんが、
	これにより測定されたランダムな信号レベルの確実な統計的推測を得ることができます。

**/
/*==============================================================================*/
/* include																		*/
/*==============================================================================*/
#include	<stdio.h>
#include	<stdlib.h>
#include	<math.h>
/*==============================================================================*/
#define		pi	((double)3.1415926535)
/*==============================================================================*/
/*	fft																			*/
/*==============================================================================*/
void fft
(
	const double	in[] ,	/* IN：実数部												*/
	int		N 	 ,	/* IN：要素数												*/
	double	ar[] ,	/* OT：実数部												*/
	double	ai[] ,	/* OT：虚数部												*/
	double	p[]		/* OT：パワー												*/
)
{
	double	ReF = 0.0;
	double	ImF = 0.0;

	// 実数部分と虚数部分に分けてフーリエ変換
	for(int n=0;n<N;n++)
	{
		ReF=ImF=0.0;
		for(int k=0;k<N;k++)
		{
			ReF+= in[k]*cos(2*pi*k*n/N);
			ImF+=-in[k]*sin(2*pi*k*n/N);
		}
		ar[n] = ReF;
		ai[n] = ImF;
		p [n] = 0;
	}
}
/*==============================================================================*/
/* EOF */